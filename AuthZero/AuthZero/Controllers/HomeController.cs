using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthZero.Controllers
{
    public class HomeController : Controller
    {
        public async Task Login(string returnURL = "/")
        {
            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() {RedirectUri = returnURL });
        }

        public async Task Logout() 
        {
            await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties()
            {
                RedirectUri = Url.Action("Index", "Home")
            });

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
