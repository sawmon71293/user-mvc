using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using user_management.Data;
using user_management.Models;
using user_management.ViewModels;

namespace user_management.Controllers
{
    [AllowAnonymous]
    public class ForgotPasswordController : Controller
    {
        
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        public ForgotPasswordController(AppDbContext context, PasswordHasher<User> passwordHasher)
        {
           _context = context;
           _passwordHasher = passwordHasher;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);
                if (user != null)
                {

                    if (model.NewPassword == model.ConfirmPassword)
                    {
                            user.Password = _passwordHasher.HashPassword(user, model.NewPassword);
                            _context.Update(user);
                            await _context.SaveChangesAsync();
                            TempData["Message"] = "Password has been reset successfully.";
                            return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                       ModelState.AddModelError(string.Empty, "New Password and Confirm Password do not match.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email not found.");
                }
                
            }
            
            return View(model);
        }
    }
}
