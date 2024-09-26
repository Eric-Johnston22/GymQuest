using GymQuest.Models;
using GymQuest.Models.ViewModels;
using GymQuest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GymQuest.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UserService _userService;
        private readonly WorkoutService _workoutService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, UserService userService, WorkoutService workoutService, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _workoutService = workoutService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation("Register action accessed");
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Login action accessed");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            _logger.LogInformation($"User creating account with email: {model.Email}");

            if (ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to User
                var user = new User
                {
                    UserName = model.Email,
                    Email =  model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CreatedAt = DateTime.Now
                };

                // Await result from User Service
                var result = await _userService.CreateUserAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    _logger.LogInformation($"{model.Email}'s account successfully created");

                    await _userService.SignInUserAsync(user, isPersistant: false);

                    // Check if the request is an AJAX request
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        // If it's an AJAx request, return success as JSON
                        return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
                    }

                    // Otherwise, return the normal redirect
                    return RedirectToAction("index", "home");
                }

                _logger.LogError($"Registration attempt failed. Errors below:");
                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    _logger.LogError($"{error.Code}: {error.Description}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Handle validation errors for AJAX requests
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return Json(new { success = false, errors });
            }

            // If it's not an AJAX request, return the standard view with errors
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"User ${model.Email} attempting to log in ...");

                var result = await _userService.PasswordSignInUserAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"{model.Email} successfully logged in");
                    // Handle successful login
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                if (result.RequiresTwoFactor)
                {
                    // Handle two-factor authentication case
                }
                if (result.IsLockedOut)
                {
                    // Handle lockout scenario
                }
                else
                {
                    // Handle failure
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation($"Logout action accessed by User: ${User.Identity.Name}");
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userData = await _userService.GetUserDataAsync(User);
            var workoutRoutines = await _workoutService.GetWorkoutRoutinesByUserAsync(userData.Id);

            _logger.LogInformation($"Profile action accessed by User ID: ${userData.Id}");

            ViewBag.FirstName = userData.FirstName;
            ViewBag.LastName = userData.LastName;
            ViewBag.Email = userData.Email;
            ViewBag.Exercises = workoutRoutines;
            ViewBag.CurrentRoutine = userData.CurrentWorkoutRoutineId;
            return View(workoutRoutines);
        }
    }
}
