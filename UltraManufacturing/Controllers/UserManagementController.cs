﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UltraManufacturing.Models.Entities;
using UltraManufacturing.Models.ViewModels;
//using UltraManufacturing.Services;
using UltraManufacturing.Filters;

namespace UltraManufacturing.Controllers
{
  
    public class UserManagementController : Controller
    {
        private readonly employeesContext _context;
        private readonly Cryptography _cryptography;

        public UserManagementController(employeesContext context, Cryptography cryptography)
        {
            _context = context;
            _cryptography = cryptography;
        }

        // GET: UserManagement
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        

        
        // GET: UserManagement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
            .SingleOrDefaultAsync(m => m.Id == id); 
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: UserManagement/Create
        public IActionResult Create()
        {
            return View();
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserManagementCreate model)
        {
            
        }

        

        // GET: UserManagement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,Email")] User model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
                    await TryUpdateModelAsync(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        // GET: UserManagement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UserManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
        }
        

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        public async Task<IActionResult> UpdatePassword(int ? id)
        {
            
        }

    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(int id, UserManagementUpdatePassword model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userCredential = await _context.UserCredential.SingleOrDefaultAsync(m => m.Id == id);
                    var passwordSalt = Guid.NewGuid().ToString();
                    userCredential.PasswordSalt = passwordSalt;
                    userCredential.HashedPassword = _cryptography.HashSHA256(model.Password + passwordSalt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["User"] = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
            return View(model);
        }

        public async Task<IActionResult> Permissions(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.Include(u => u.UserPermission).SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["User"] = user;
            ViewData["Permissions"] = await _context.Permission.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPermission(int userId, int permissionId)
        {
            var userPermission = new UserPermission()
            {
                UserId = userId,
                PermissionId = permissionId,
            };
            _context.UserPermission.Add(userPermission);
            _context.SaveChanges();
            return RedirectToAction("Permissions", new { id = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemovePermission(int userId, int permissionId)
        {
            var userPermission = _context.UserPermission.Single(up => up.PermissionId == permissionId && up.UserId == userId);
            _context.UserPermission.Remove(userPermission);
            _context.SaveChanges();
            return RedirectToAction("Permissions", new { id = userId });
        }
        
    }

    internal class UMAuthenticationAttribute : Attribute
    {
        private string v;

        public UMAuthenticationAttribute(string v)
        {
            this.v = v;
        }

      
    }
}
