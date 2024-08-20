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

        // Step 1: Create routine (basic info)
        [HttpGet]
        public IActionResult CreateRoutine()
        {
            return View(new CreateRoutineViewModel());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoutine(CreateRoutineViewModel model)
        {
            int workoutRoutineId;
            if (ModelState.IsValid)
            {
                try
                {
                    workoutRoutineId = await _workoutService.CreateRoutineAsync(model, User);
                }
                catch (Exception err)
                {
                    return View(err);
                }
                // Redirect to the next step, passing the WorkoutRoutineId
                return RedirectToAction("AddWorkoutDays", new { id = workoutRoutineId });
            }

            return View(model);
        }

        // Step 2: Add Workout Days
        [HttpGet]
        public async Task<IActionResult> AddWorkoutDays(int id)
        {
            var workoutRoutine = await _workoutService.GetWorkoutRoutineByIdAsync(id);
            if (workoutRoutine == null || workoutRoutine.Status != "Draft")
            {
                return NotFound();
            }
            var model = new AddWorkoutDaysViewModel { WorkoutRoutineId = id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWorkoutDays(AddWorkoutDaysViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _workoutService.AddWorkoutDaysAsync(model);
                return RedirectToAction("AssignExercises", new { id = model.WorkoutRoutineId });
            }
            return View(model);
        }

        // Step 3: Assign Exercises
        [HttpGet]
        public async Task<IActionResult> AssignExercises(int id)
        {
            var workoutRoutine = await _workoutService.GetWorkoutRoutineByIdAsync(id);
            if (workoutRoutine == null || workoutRoutine.Status != "Draft")
            {
                return NotFound();
            }
            
            // Get a list of exercises
            var exercises = await _workoutService.GetExercisesAsync();

            // Map WorkoutDays to WorkoutDayExercisesViewModel
            var model = new AssignExercisesViewModel
            {
                WorkoutRoutineId = id,
                WorkoutDays = workoutRoutine.WorkoutDays.Select(day => new AssignExercisesViewModel.WorkoutDayExercisesViewModel
                {
                    WorkoutDayId = day.WorkoutDayId,
                    DayName = day.DaysOfWeek!.DayName!, // Assuming DaysOfWeek is a navigation property
                    PlannedExercises = day.PlannedExercises.Select(exercise => new AssignExercisesViewModel.WorkoutDayExercisesViewModel.PlannedExerciseViewModel
                    {
                        ExerciseId = exercise.ExerciseId,
                        Sets = exercise.Sets,
                        Reps = exercise.Reps,
                        Weight = exercise.Weight,
                        Notes = exercise.Notes
                    }).ToList()
                }).ToList()
            };

            ViewBag.Exercises = exercises;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignExercises(AssignExercisesViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _workoutService.AssignExercisesAsync(model);
                return RedirectToAction("ReviewRoutine", new { id = model.WorkoutRoutineId });
            }

            return View(model);
        }

        // Step 4: Review and confirm
        [HttpGet]
        public async Task<IActionResult> ReviewRoutine(int id)
        {
            var workoutRoutine = await _workoutService.GetWorkoutRoutineByIdAsync(id);
            if (workoutRoutine == null || workoutRoutine.Status != "Draft")
            {
                return NotFound();
            }

            var model = new ReviewRoutineViewModel { WorkoutRoutine = workoutRoutine };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmRoutine(int id)
        {
            await _workoutService.CompleteRoutineAsync(id);
            return RedirectToAction("ViewRoutine", new { id = id });
        }

        // View Completed Routine
        [HttpGet]
        public async Task<IActionResult> ViewRoutine(int id)
        {
            var workoutRoutine = await _workoutService.GetWorkoutRoutineByIdAsync(id);
            if (workoutRoutine == null || workoutRoutine.Status != "Completed")
            {
                return NotFound();
            }
            return View(workoutRoutine);
        }
    }
}
