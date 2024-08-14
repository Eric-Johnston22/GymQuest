using GymQuest.DataAccess;
using GymQuest.Services;
using GymQuest.Models;
using GymQuest.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymQuest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly UserService _userService;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, UserService userService)
        {
            _logger = logger;
            _userManager = userManager;
            _userService = userService;
        }

        

        public async Task<IActionResult> Index()
        {
            string firstName = await _userService.GetFirstNameAsync(User);
            ViewBag.FirstName = firstName;

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
