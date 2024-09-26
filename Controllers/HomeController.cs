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
        private readonly WorkoutService _workoutService;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, UserService userService, WorkoutService workoutService)
        {
            _logger = logger;
            _userManager = userManager;
            _userService = userService;
            _workoutService = workoutService;
        }

        

        public async Task<IActionResult> Index()
        {
            // Get the logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogInformation("Index action accessed by logged out User");
                return View(); // Return an empty view if no user is logged in
            }

            _logger.LogInformation($"Index action accessed by User ID: {user.Id}");
            // Retrive current routine for logged-in user
            var currentRoutine = await _workoutService.GetWorkoutRoutineByIdAsync(user.CurrentWorkoutRoutineId);

            // Create the view model
            var model = new HomeViewModel
            {
                FirstName = user.FirstName,
                CurrentRoutine = currentRoutine
            };

            return View(model);
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
