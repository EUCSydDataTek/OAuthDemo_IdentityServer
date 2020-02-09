using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pluralsight.Client.M5.Oidc
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
        
        [Authorize]
        public ActionResult Login()
        {
            return Redirect("Index");
        }

        public ActionResult Logout()
        {
            return SignOut();
        }
    }
}
