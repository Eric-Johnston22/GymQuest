using GymQuest.DataAccess;
using GymQuest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymQuest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDAO _userDAO;

        public HomeController(ILogger<HomeController> logger, UserDAO userDAO)
        {
            _logger = logger;
            _userDAO = userDAO;
        }

        public IActionResult Index()
        {
            var getFirstUser = _userDAO.GetFirst();
            return View(getFirstUser);
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
