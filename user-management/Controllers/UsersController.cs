using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_management.Data;
using user_management.DTOs;
using user_management.Models;
using user_management.ViewModels;

namespace user_management.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var viewModel = new UserCreateViewModel
            {
                NewUser = new User(),
                Users = await _context.Users
                        .ToListAsync()
            };
            return View(viewModel);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            var oneHourAgo = DateTime.UtcNow.AddHours(-1);
            Console.WriteLine(oneHourAgo);
            var recentUsers = await _context.Users
                        .Where(u => u.CreatedAt >= oneHourAgo)
                        .ToListAsync();
            var viewModel = new UserCreateViewModel
            {
                NewUser = new User(),
                Users = recentUsers ?? new List<User>()
            };
            return View(viewModel);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel userViewModel)
        {
            // Always fetch recent users for the view
            var oneHourAgo = DateTime.UtcNow.AddHours(-1);
            userViewModel.Users = await _context.Users
                        .Where(u => u.CreatedAt >= oneHourAgo)
                        .ToListAsync();
            var user = userViewModel.NewUser;
            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            // Check for duplicates
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username || u.Email == user.Email);

            if (existingUser != null)
            {
                if (existingUser.Username == user.Username)
                    ModelState.AddModelError("NewUser.Username", "Duplicate Username!");
                if (existingUser.Email == user.Email)
                    ModelState.AddModelError("NewUser.Email", "Duplicate Email!");

                return View(userViewModel);
            }

            // Set CreatedAt to UTC
            user.CreatedAt = DateTime.UtcNow;

            _context.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            UserUpdateDto updatingUser = new UserUpdateDto();
            updatingUser.Id = user.Id;
            updatingUser.Username = user.Username;
            updatingUser.Email = user.Email;
            updatingUser.ContactNo = user.ContactNo;
            updatingUser.NRCNo = user.NRCNo;
          
            return View(updatingUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Email,ContactNo,NRCNo")]UserUpdateDto userDto)
        {

            if (!ModelState.IsValid)
            {
                return View(userDto);
            }

            User updateUser = await _context.Users.FindAsync(id);
            if (updateUser == null)
            { 
               return NotFound();
            }

            User existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id != userDto.Id && ( u.Username == userDto.Username || u.Email == userDto.Email));
            if (existingUser != null)
            {
                if (existingUser.Username == userDto.Username && existingUser.Id != id)
                    ModelState.AddModelError("Username", "Duplicate Username!");
                if (existingUser.Email == userDto.Email && existingUser.Id != id)
                    ModelState.AddModelError("Email", "Duplicate Email!");
                return View(userDto);
            }


            try
                {
                    updateUser.Username = userDto.Username;
                    updateUser.Email = userDto.Email;
                    updateUser.ContactNo = userDto.ContactNo;
                    updateUser.NRCNo = userDto.NRCNo;
                    updateUser.UpdateAt = DateTime.UtcNow;  
                    _context.Update(updateUser);
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

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
