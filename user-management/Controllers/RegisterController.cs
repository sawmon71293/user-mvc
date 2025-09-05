using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using user_management.Data;
using user_management.Models;

namespace user_management.Controllers
{
    public class RegisterController(AppDbContext context, PasswordHasher<User> passwordHasher) : Controller
    {   
        private readonly AppDbContext _context = context;
        private readonly PasswordHasher<User> _passwordHasher = passwordHasher;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Id,Username,Email,ContactNo,NRCNo,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username || u.Email == user.Email);
                if (existingUser != null)
                {
                    if (existingUser.Username == user.Username)
                        ModelState.AddModelError("Username", "Duplicate Username!");
                    if (existingUser.Email == user.Email)
                        ModelState.AddModelError("Email", "Duplicate Email!");
                    return View(user);
                }
                user.Password = _passwordHasher.HashPassword(user, user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                TempData["Message"] = "User has been registered successfully.";
                return RedirectToAction("Index","Login");
            }
            return View(user);
        }

    }
}
