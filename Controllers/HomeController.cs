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
            ViewBag.UserType = Request.Cookies["UserType"];
            if (ViewData["UserType"] == null)
            {
                return View();
            }
            else if (ViewBag.UserType == "Admin")
            {
                return RedirectToAction("IndexAdmin");
            }
            else
            {
                return RedirectToAction("IndexUser");
            }
        }
        public IActionResult IndexAdmin()
        {
            ViewData["UserType"] = Request.Cookies["UserType"];
            return View();
        }
        public IActionResult IndexUser()
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
                        return RedirectToAction("IndexAdmin", "Home");
                        //return RedirectToAction("Index", "Users");
                    }
                    else
                    {
                        CookieOptions option = new CookieOptions();
                        httpContextAccessor.HttpContext.Response.Cookies.Append("UserType", "User", option);
                        httpContextAccessor.HttpContext.Response.Cookies.Append("CurrentUserId", user.UserId.ToString(), option);
                        return RedirectToAction("IndexUser", "Home");
                        //return RedirectToAction("Index", "Bookings");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Append("UserType", "", new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(-1)
            });
            Response.Cookies.Append("CurrentUserId", "", new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(-1)
            });
            return RedirectToAction("Index");
        }

        public IActionResult Contact()
        {
            return View();
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