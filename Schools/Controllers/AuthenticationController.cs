using Microsoft.AspNetCore.Mvc;

namespace Schools.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
