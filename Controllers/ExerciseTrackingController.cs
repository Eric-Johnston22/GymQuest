using GymQuest.Models.ViewModels;
using GymQuest.Models;
using GymQuest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace GymQuest.Controllers
{
    public class ExerciseTrackingController : Controller
    {
        private readonly WorkoutService _workoutService;
        private readonly ExerciseTrackingService _exerciseTrackingService;
        private readonly UserService _userService;

        public ExerciseTrackingController(WorkoutService workoutService, ExerciseTrackingService exerciseTrackingService, UserService userService)
        {
            _workoutService = workoutService;
            _exerciseTrackingService = exerciseTrackingService;
            _userService = userService;
        }

        // POST: Start a workout session based on the current day
        [HttpPost, ActionName("StartRoutine")]
        public async Task<IActionResult> StartRoutinePost(int routineId)
        {
            var workoutRoutine = await _workoutService.GetWorkoutRoutineByIdAsync(routineId);
            if (workoutRoutine == null)
            {
                return NotFound();
            }

            // Fetch the currently logged-in user
            var userId = await _userService.GetUserIdAsync(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); // Handle unauthorized access
            }

            // Update the user's current routine
            await _exerciseTrackingService.SetCurrentRoutineAsync(userId, routineId);


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
                    PlannedExercisesId = ex.PlannedExercisesId,
                    ExerciseName = ex.Exercises.Name,
                    Sets = ex.Sets,
                    SetNumber = 0, // Initialize to 0, increment as needed
                    GoalReps = ex.Reps,
                    Weight = ex.Weight,
                    Notes = null
                }).ToList()
            };

            return View(model);
        }

        // GET: Start a workout session based on the current day
        [HttpGet, ActionName("StartRoutine")]
        public async Task<IActionResult> StartRoutineGet(int routineId)
        {
            var workoutRoutine = await _workoutService.GetWorkoutRoutineByIdAsync(routineId);
            if (workoutRoutine == null)
            {
                return NotFound();
            }

            // Fetch the currently logged-in user
            var userId = await _userService.GetUserIdAsync(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); // Handle unauthorized access
            }

            // Update the user's current routine
            await _exerciseTrackingService.SetCurrentRoutineAsync(userId, routineId);


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
                    PlannedExercisesId = ex.PlannedExercisesId,
                    ExerciseName = ex.Exercises.Name,
                    Sets = ex.Sets,
                    SetNumber = 0, // Initialize to 0, increment as needed
                    GoalReps = ex.Reps,
                    Weight = ex.Weight,
                    Notes = null
                }).ToList()
            };

            return View(model);
        }

        // Log the exercises for the current session
        [HttpPost]
        public async Task<IActionResult> LogExercise(int plannedExercisesId, int goalSets, int goalReps, int weight, int setNumber, int repsCompleted, string? notes)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Log for debugging
            Console.WriteLine($"\n\nLogging Exercise: PlannedExercisesId: {plannedExercisesId}, GoalReps: {goalReps}, Weight: {weight}, SetNumber: {setNumber}, RepsCompleted: {repsCompleted}, Notes: {notes}\n\n");

            var exerciseLog = new ExerciseLogs
            {
                PlannedExercisesId = plannedExercisesId,
                UserId = userId,
                Date = DateTime.Now,
                Sets = goalSets,
                Reps = goalReps,
                Weight = weight,
                SetNumber = setNumber,
                RepsCompleted = repsCompleted,
                Notes = notes, // Save notes
            };

            await _exerciseTrackingService.LogExerciseAsync(exerciseLog);

            return Ok(); // Indicate success
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
        public async Task<IActionResult> AddExerciseToDay(int dayId, int exerciseId, int sets, int reps, decimal weight, string notes)
        {
            // Logic to save the exercise
            var newExercise = new PlannedExercises
            {
                WorkoutDayId = dayId,
                ExerciseId = exerciseId,
                Sets = sets,
                Reps = reps,
                Weight = weight,
                Notes = notes
            };

            await _exerciseTrackingService.AddPlannedExerciseAsync(newExercise);

            return Ok();
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

        [HttpPost]
        public async Task<IActionResult> EndWorkout(int routineId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workoutSummary = await _exerciseTrackingService.GetWorkoutSummaryAsync(userId, routineId);

            return View("WorkoutSummary", workoutSummary);
        }

    }
}
