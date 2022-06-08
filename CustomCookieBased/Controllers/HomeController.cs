using CustomCookieBased.Data;
using CustomCookieBased.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CustomCookieBased.Controllers
{
    public class HomeController : Controller
    {
        private readonly CookieContext _context;

        public HomeController(CookieContext context)
        {
            _context = context;
        }

        public IActionResult SignIn()
        {
            return View(new UserSignInModel());
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel model)
        {
            var user = _context.Users.SingleOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
            if (user != null)
            {
                var roles = _context.Roles.Where(x => x.UserRoles.Any(x => x.UserId == user.Id)).Select(x=>x.Defination).ToList();
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,model.UserName),
                    
                };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.Remember
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
            return View(model);


        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
