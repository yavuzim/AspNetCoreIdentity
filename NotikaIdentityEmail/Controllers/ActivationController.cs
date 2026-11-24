using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;

namespace NotikaIdentityEmail.Controllers
{
    public class ActivationController : Controller
    {
        private readonly EmailContext _context;

        public ActivationController(EmailContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult UserActivation()
        {
            TempData.Keep("MoveEmail");  
            return View();
        }

        [HttpPost]
        public IActionResult UserActivation(int userCodeParameter)
        {
            string email = TempData.Peek("MoveEmail")?.ToString();
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("CreateUser", "Register");

            var code = _context.Users.Where(x => x.Email == email).Select(y => y.ActivtionCode).FirstOrDefault();

            if (userCodeParameter == code)
            {
                var value = _context.Users.Where(x => x.Email == email).FirstOrDefault();
                value.EmailConfirmed = true;
                _context.SaveChanges();
                return RedirectToAction("UserLogin", "Login");
            }
            return View();
        }
    }
}
