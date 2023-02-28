using Biluthyrning.Data;
using Biluthyrning.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Biluthyrning.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUser userRepo;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IUser userRepo, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            this.userRepo = userRepo;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            ViewData["UserType"] = Request.Cookies["UserType"];
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            foreach (var item in await userRepo.GetAllAsync())
            {

                if (item.UserName == user.UserName && item.Password == user.Password)
                {
                    user = item;
                    if (user.IsAdmin == true)
                    {
                        CookieOptions option = new CookieOptions();
                        httpContextAccessor.HttpContext.Response.Cookies.Append("UserType", "Admin", option);
                        return RedirectToAction("Index", "Users");
                    }
                    else
                    {
                        CookieOptions option = new CookieOptions();
                        httpContextAccessor.HttpContext.Response.Cookies.Append("UserType", "User", option);
                        return RedirectToAction("Index", "Bookings");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}