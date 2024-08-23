using GymQuest.Models.ViewModels;
using GymQuest.Models;
using GymQuest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymQuest.Controllers
{
    public class ExerciseTrackingController : Controller
    {
        private readonly WorkoutService _workoutService;
        private readonly ExerciseTrackingService _exerciseTrackingService;

        public ExerciseTrackingController(WorkoutService workoutService, ExerciseTrackingService exerciseTrackingService)
        {
            _workoutService = workoutService;
            _exerciseTrackingService = exerciseTrackingService;
        }

        // Start a workout session based on the current day
        [HttpGet]
        public async Task<IActionResult> StartRoutine(int routineId)
        {
            var workoutRoutine = await _workoutService.GetWorkoutRoutineByIdAsync(routineId);
            if (workoutRoutine == null)
            {
                return NotFound();
            }

            var currentDayOfWeek = DateTime.Now.DayOfWeek.ToString();
            var exercises = await _workoutService.GetExercisesAsync();
            ViewBag.Exercises = exercises;

            var workoutDay = workoutRoutine.WorkoutDays
                .FirstOrDefault(day => day.DaysOfWeek.DayName == currentDayOfWeek);

            if (workoutDay == null)
            {
                // Pass a flag indicating no exercises are planned for the day
                var startRoutineModel = new StartRoutineViewModel
                {
                    WorkoutRoutineId = workoutRoutine.WorkoutRoutineId,
                    RoutineName = workoutRoutine.RoutineName,
                    DayName = workoutDay?.DaysOfWeek?.DayName ?? currentDayOfWeek,
                    PlannedExercises = new List<ExerciseLogViewModel>() // No planned exercises
                };

                ViewBag.NoExercisesPlanned = true; // Set a flag to show the option to add exercises

                return View(startRoutineModel);
            }

            var model = new StartRoutineViewModel
            {
                WorkoutRoutineId = workoutRoutine.WorkoutRoutineId,
                RoutineName = workoutRoutine.RoutineName,
                DayName = workoutDay.DaysOfWeek.DayName,
                PlannedExercises = workoutDay.PlannedExercises.Select(ex => new ExerciseLogViewModel
                {
                    PlannedExerciseId = ex.PlannedExercisesId,
                    ExerciseName = ex.Exercises.Name,
                    SetNumber = 0, // Initialize to 0, increment as needed
                    Reps = ex.Reps,
                    Weight = ex.Weight
                }).ToList()
            };

            return View(model);
        }

        // Log the exercises for the current session
        [HttpPost]
        public async Task<IActionResult> LogExercises(StartRoutineViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                foreach (var log in model.PlannedExercises)
                {
                    for (int set = 1; set <= log.Sets; set++)
                    {
                        var exerciseLog = new ExerciseLogs
                        {
                            PlannedExerciseId = log.PlannedExerciseId,
                            UserId = userId,
                            Date = DateTime.Now,
                            SetNumber = set,
                            RepsCompleted = log.IsSuccessful ? log.Reps : log.FailedRep ?? 0,
                            Weight = log.Weight,
                            IsSuccessful = log.IsSuccessful
                        };

                        await _exerciseTrackingService.LogExerciseAsync(exerciseLog);
                    }
                }

                return RedirectToAction("ViewRoutine", "Workout", new { id = model.WorkoutRoutineId });
            }

            return View("StartRoutine", model);
        }

        // Allow users to add new exercises to an existing day
        [HttpPost]
        public async Task<IActionResult> AddExercise(int workoutRoutineId, string dayName, string exerciseName, int sets, int reps, decimal weight)
        {
            var workoutRoutine = await _workoutService.GetWorkoutRoutineByIdAsync(workoutRoutineId);
            if (workoutRoutine == null)
            {
                return NotFound();
            }

            var workoutDay = workoutRoutine.WorkoutDays
                .FirstOrDefault(day => day.DaysOfWeek.DayName == dayName);

            if (workoutDay == null)
            {
                return NotFound("Invalid day selected.");
            }

            var newExercise = new PlannedExercises
            {
                WorkoutDayId = workoutDay.WorkoutDayId,
                ExerciseId = await _exerciseTrackingService.GetOrCreateExerciseAsync(exerciseName),
                Sets = sets,
                Reps = reps,
                Weight = weight
            };

            await _exerciseTrackingService.AddPlannedExerciseAsync(newExercise);

            return RedirectToAction("StartRoutine", new { routineId = workoutRoutineId });
        }

        [HttpPost]
        public async Task<IActionResult> AddExerciseToDay(int workoutRoutineId, string dayName, int exerciseId, int sets, int reps, decimal weight, string? notes)
        {
            var workoutRoutine = await _workoutService.GetWorkoutRoutineByIdAsync(workoutRoutineId);
            if (workoutRoutine == null)
            {
                return NotFound();
            }

            var workoutDay = workoutRoutine.WorkoutDays
                .FirstOrDefault(day => day.DaysOfWeek.DayName == dayName);

            if (workoutDay == null)
            {
                return NotFound("Invalid day selected.");
            }

            var newExercise = new PlannedExercises
            {
                WorkoutDayId = workoutDay.WorkoutDayId,
                ExerciseId = exerciseId,
                Sets = sets,
                Reps = reps,
                Weight = weight,
                Notes = notes
            };

            await _exerciseTrackingService.AddPlannedExerciseAsync(newExercise);

            return Json(new { exerciseName = newExercise.Exercises?.Name });
        }

        [HttpPost]
        public async Task<IActionResult> CreateExercise(string name, string? description)
        {
            var exercise = new Exercises
            {
                Name = name,
                Description = description
            };

            await _workoutService.CreateExercise(exercise);

            return Json(new { exerciseId = exercise.ExerciseId, name = exercise.Name });
        }


    }
}
