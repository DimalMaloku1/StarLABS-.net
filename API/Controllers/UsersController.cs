using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("Users")]
    public class UsersController : Controller
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: Users/Index
        public IActionResult Index()
        {
            List<AppUser> users = _context.AppUsers.ToList();
            return View("Index", users);
        }

        // POST: Users/Delete/{userId}
        [HttpPost]
        public IActionResult Delete(Guid userId)
        {
            var user = _context.AppUsers.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                _context.AppUsers.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Users/Edit/{userId}
        [HttpGet("Edit/{userId}")]
        public IActionResult Edit(Guid userId)
        {
            var user = _context.AppUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost("Edit/{userId}")]
        public IActionResult Edit(Guid userId, AppUser user)
        {
            if (ModelState.IsValid)
            {
                var originalUser = _context.AppUsers.FirstOrDefault(u => u.Id == userId);
                if (originalUser == null)
                {
                    return NotFound();
                }

                originalUser.UserName = user.UserName;
                originalUser.firstName = user.firstName;
                originalUser.lastName = user.lastName;
                originalUser.Role = user.Role;

                try
                {
                    _context.Update(originalUser);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError("", "Concurrency conflict: " + ex.Message);
                    return View(originalUser);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating user: " + ex.Message);
                    return View(originalUser);
                }
            }
            return View(user);
        }


    }
}
