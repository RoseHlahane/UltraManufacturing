using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UltraManufacturing.Models;
using UltraManufacturing.Models.Entities;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;


namespace UltraManufacturing.Services
{
    public class UMAuthentication
    {
        readonly HttpContext _httpContext;
        readonly Cryptography _cryptography;
        readonly AuthOptions _authenticationConfiguration;
        readonly employeesContext _context;

        public UMAuthentication(
        IHttpContextAccessor contextAccessor,
        Cryptography cryptography,
        IOptions<AuthOptions> authenticationConfiguration,
        employeesContext context)
        {
            _httpContext = contextAccessor.HttpContext;
            _cryptography = cryptography;
            _authenticationConfiguration = authenticationConfiguration.Value;
            _context = context;

        }

        private AuthInfo _scopeAuthInfo = null;
        public AuthInfo ScopeAuthInfo
        {
            get
            {
                if (_scopeAuthInfo == null)
                {
                    AuthInfo tokenAuthInfo = null;
                    var cookieValue = _httpContext.Request.Cookies["UMAuthToken"];
                    if (!string.IsNullOrEmpty(cookieValue))
                    {
                        try
                        {
                            tokenAuthInfo = AuthInfoFromToken(cookieValue);
                        }
                        catch
                        {
                        }
                    }
                    _scopeAuthInfo = tokenAuthInfo != null ? tokenAuthInfo : new AuthInfo();
                }
                return _scopeAuthInfo;
            }

        }

        public bool SignIn(string email, string password)
        {
            var user = _context.User.Include(u => u.UserCredential).FirstOrDefault(u => u.Email == email);

            if (user == null) return false;
            var userCredential = user.UserCredential;
            var claimedPasswordHashed = _cryptography.HashSHA256(password + userCredential.PasswordSalt);

            if (claimedPasswordHashed != userCredential.HashedPassword) return false;

            var permissions = _context.UserPermission.Where(up => up.UserId == user.Id)
                           .Select(up => up.Permission.Code).ToList();

            AuthInfo authInfo = new AuthInfo()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Claims = new Dictionary<string, string>(),
                Permissions = permissions,
            };


            _httpContext.Response.Cookies.Append("UMAuthToken", AuthInfoToToken(authInfo));

            return true;

        }

        public void SignOut()
        {
            _httpContext.Response.Cookies.Delete("UMAuthToken");
        }

        private string AuthInfoToToken(AuthInfo authInfo)
        {
            var serializedAuthInfo = JsonConvert.SerializeObject(authInfo);

            var key = Encoding.UTF8.GetBytes(_authenticationConfiguration.AuthEncryptionKey);
            var iv = Aes.Create().IV;
            var ivBase64 = Convert.ToBase64String(iv);
            var encryptBytes = _cryptography.EncryptStringToBytes_Aes(serializedAuthInfo, key, iv);
            var result = $"{ivBase64.Length.ToString().PadLeft(3, '0')}{ivBase64}{Convert.ToBase64String(encryptBytes)}";
            return result;
        }

        private AuthInfo AuthInfoFromToken(string token)
        {

            string decryptedToken;
            var ivLength = Convert.ToInt32(token.Substring(0, 3));
            var ivBase64 = token.Substring(3, ivLength);
            var iv = Convert.FromBase64String(ivBase64);
            var encryptBase64 = token.Substring(ivLength + 3);
            var encryptBytes = Convert.FromBase64String(encryptBase64);
            var key = Encoding.UTF8.GetBytes(_authenticationConfiguration.AuthEncryptionKey);
            decryptedToken = _cryptography.DecryptStringFromBytes_Aes(encryptBytes, key, iv);


            var result = JsonConvert.DeserializeObject<AuthInfo>(decryptedToken);
            return result;
        }
    }
}

