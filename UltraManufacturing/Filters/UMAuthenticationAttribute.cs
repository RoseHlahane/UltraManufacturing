using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UltraManufacturing.Filters
{
    public class UMAuthenticationAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        readonly List<string> _permissions;

        public UMAuthenticationAttribute(string permissions = "")
        {
            _permissions = permissions.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .ToList();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var service = context.HttpContext.RequestServices.GetService(typeof(Services.UMAuthentication)) as Services.UMAuthentication;

            if (service.ScopeAuthInfo.IsAuthenticated && !_permissions.Any())
            {
                return;
            }

            if (!service.ScopeAuthInfo.IsAuthenticated || !_permissions.Any(p => service.ScopeAuthInfo.Permissions.Contains(p)))
            {
                var returnUrl = context.HttpContext.Request.Path;
                // context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = returnUrl });
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl });
            }
        }
    }
}
