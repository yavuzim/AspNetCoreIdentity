using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models;

namespace NotikaIdentityEmail.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterUserViewModel model)
        {
            AppUser appUser = new AppUser();
            appUser.Name = model.Name;
            appUser.Surname = model.Surname;
            appUser.Email = model.Email;
            appUser.UserName = model.Username;


            var result = await _userManager.CreateAsync(appUser, model.Password);
            if (result.Succeeded)
                return RedirectToAction("UserLogin", "Login");
            else
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            return View();
        }
    }
}
