using GymQuest.DataAccess;
using GymQuest.Services;
using GymQuest.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GymQuest.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GymQuest.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly UserService _userService;
        private readonly WorkoutService _workoutService;

        public WorkoutController(UserService userService, WorkoutService workoutService)
        {
            _userService = userService;
            _workoutService = workoutService;
        }

        // routine creation
        [HttpGet]
        public IActionResult CreateRoutine()
        {
            return View(new CreateRoutineViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoutine(CreateRoutineViewModel model)
        {
            int routineId;
            if (ModelState.IsValid)
            {
                try
                {
                    await _workoutService.CreateRoutineAsync(model, User);
                    routineId = await _workoutService.GetRoutineIdAsync();
                }
                catch (Exception err)
                {
                    return View(err);
                }
                // Redirect to the next step, passing the WorkoutRoutineId
                return RedirectToAction("AddWorkoutDays", new { id = workoutRoutine.WorkoutRoutineId });
            }

            return View(model);
        }

    }
}
