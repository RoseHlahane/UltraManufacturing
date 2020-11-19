using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UltraManufacturing.Models.ViewModels;
using UltraManufacturing.Services;
using UltraManufacturing.Models.Entities;

namespace UltraManufacturing.Controllers
{
    public class UserRegistrationController : Controller
    {
        private readonly UMAuthentication _emAuth;
        private readonly employeesContext _employeeRegistration;
        private readonly Cryptography _cryptography;

        public UserRegistrationController(employeesContext employeeRegistration, Cryptography cryptography, UMAuthentication emAuth)
        {
            _emAuth = emAuth;
            _employeeRegistration = employeeRegistration;
            _cryptography = cryptography;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _employeeRegistration.User.ToListAsync());
        }

        
        public IActionResult Create(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserManagementCreate model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    CreationDate = DateTime.UtcNow,
                };

                var passwordSalt = Guid.NewGuid().ToString();
                var userCredential = new UserCredential()
                {
                    PasswordSalt = passwordSalt,
                    HashedPassword = _cryptography.HashSHA256(model.Password + passwordSalt),
                };

                user.UserCredential = userCredential;

                _employeeRegistration.Add(user);
                await _employeeRegistration.SaveChangesAsync();
                ViewBag.message = "The User " + model.FirstName + " Is Saved Successfully..!";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }
}