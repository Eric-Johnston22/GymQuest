﻿using GymQuest.DataAccess;
using GymQuest.Services;
using GymQuest.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GymQuest.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Diagnostics;

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
                    // Redirect to the next step, passing the WorkoutRoutineId
                    return RedirectToAction("AddWorkoutDays", new { id = workoutRoutineId });
                }
                catch (Exception err)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the routine. Please try again.");
                }
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
            var model = new AddWorkoutDaysViewModel 
            { 
                WorkoutRoutineId = id,
                WorkoutDays = new List<AddWorkoutDaysViewModel.WorkoutDayViewModel>
                {
                    new AddWorkoutDaysViewModel.WorkoutDayViewModel()
                }
            
            };
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
            
            // Get a list of exercises to be used in the dropdown list
            var exercises = await _workoutService.GetExercisesAsync();
            ViewBag.Exercises = exercises;

            // Map WorkoutDays to WorkoutDayExercisesViewModel
            var model = new AssignExercisesViewModel
            {
                WorkoutRoutineId = id,
                WorkoutDays = workoutRoutine.WorkoutDays.Select(day => new AssignExercisesViewModel.WorkoutDayExercisesViewModel
                {
                    WorkoutDayId = day.WorkoutDayId,
                    DayName = day.DaysOfWeek?.DayName ?? "Unknown Day",
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

        [HttpPost]
        public async Task<IActionResult> CreateExercise(string name, string description)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Exercise name is required.");
            }

            var newExercise = new Exercises
            {
                Name = name,
                Description = description
            };

            await _workoutService.CreateExercise(newExercise);

            return Json(new { exerciseId = newExercise.ExerciseId, name = newExercise.Name });
        }

        public async Task<IActionResult> AddExerciseToDay(AssignExercisesViewModel.WorkoutDayExercisesViewModel.PlannedExerciseViewModel model, int workoutDayId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _workoutService.AddExerciseToDayAsync(model, workoutDayId);

                    if (result.Success)
                    {
                        Console.WriteLine("Add exercise to day success!");
                        return Json(new { exerciseName = result.ExerciseName });
                    }
                    else
                    {
                        return StatusCode(500, result.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine("Error in AddExerciseToDay POST method: " + ex.Message);
                    return StatusCode(500, "Internal server error");
                }
            }

            return BadRequest("Invalid data provided.");
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
            if (id == 0)
            {
                Debug.WriteLine("WorkoutRoutineId is Zero in ConfirmRoutine()");
            }
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

            var model = new ViewRoutineViewModel
            {
                WorkoutRoutineId = workoutRoutine.WorkoutRoutineId,
                RoutineName = workoutRoutine.RoutineName,
                CycleDays = workoutRoutine.CycleDays,
                IsCycle = workoutRoutine.IsCycle,
                WorkoutDays = workoutRoutine.WorkoutDays.Select(day => new ViewRoutineDayViewModel
                {
                    DayName = day.DaysOfWeek?.DayName,
                    Exercises = day.PlannedExercises.Select(ex => new ViewRoutineExerciseViewModel
                    {
                        ExerciseName = ex.Exercises?.Name,
                        Sets = ex.Sets,
                        Reps = ex.Reps,
                        Weight = ex.Weight,
                        Notes = ex.Notes
                    }).ToList()
                }).ToList()
            };

            return View(model);
        }

    }
}