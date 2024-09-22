using GymQuest.Data;
using GymQuest.Models;
using GymQuest.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Claims;
using static GymQuest.Models.ViewModels.CreateRoutineViewModel;

namespace GymQuest.Services
{
    public class WorkoutService
    {
        private readonly WorkoutRepository _workoutRepository;
        private readonly UserManager<User> _userManager;

        public WorkoutService(WorkoutRepository workoutRepository, UserManager<User> userManager)
        {
            _workoutRepository = workoutRepository;
            _userManager = userManager;
        }

        public async Task<int> CreateRoutineAsync(CreateRoutineViewModel model, ClaimsPrincipal user)
        {

            try
            {
                var userId = _userManager.GetUserId(user);
                var workoutRoutine = new WorkoutRoutines
                {
                    RoutineName = model.RoutineName,
                    CycleDays = model.CycleDays,
                    IsCycle = model.IsCycle,
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    Status = "Draft" // Set as draft
                };

                return await _workoutRepository.CreateWorkoutRoutineAsync(workoutRoutine);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("There was an error saving the workout routine", ex);
            }
        }

        public async Task AddWorkoutDaysAsync(AddWorkoutDaysViewModel model)
        {
            var workoutRoutine = await _workoutRepository.GetWorkoutRoutineByIdAsync(model.WorkoutRoutineId);
            if (workoutRoutine != null && workoutRoutine.Status == "Draft")
            {
                foreach (var day in model.WorkoutDays)
                {
                    workoutRoutine.WorkoutDays.Add(new WorkoutDays
                    {
                        DayId = day.DayId,
                        WorkoutRoutineId = model.WorkoutRoutineId,
                        WorkoutType = day.WorkoutType
                    });
                }

                await _workoutRepository.UpdateWorkoutRoutineAsync(workoutRoutine);
            }
        }

        public async Task AssignExercisesAsync(AssignExercisesViewModel model)
        {
            var workoutRoutine = await _workoutRepository.GetWorkoutRoutineByIdAsync(model.WorkoutRoutineId);
            if (workoutRoutine != null && workoutRoutine.Status == "Draft")
            {
                foreach (var day in model.WorkoutDays)
                {
                    var workoutDay = workoutRoutine.WorkoutDays.First(wd => wd.WorkoutDayId == day.WorkoutDayId);

                    foreach (var exercise in day.PlannedExercises)
                    {
                        workoutDay.PlannedExercises.Add(new PlannedExercises
                        {
                            ExerciseId = exercise.ExerciseId,
                            Sets = exercise.Sets,
                            Reps = exercise.Reps,
                            Weight = exercise.Weight,
                            Notes = exercise.Notes
                        });
                    }
                }

                await _workoutRepository.UpdateWorkoutRoutineAsync(workoutRoutine);
            }
        }

        public async Task CreateExercise(Exercises exercise)
        {
            await _workoutRepository.CreateExercise(exercise);
        }

        public async Task<(bool Success, string ExerciseName, string ErrorMessage)> AddExerciseToDayAsync(
            AssignExercisesViewModel.WorkoutDayExercisesViewModel.PlannedExerciseViewModel model, int workoutDayId)
        {
            try
            {
                // Business logic: validate inputs, check if the workout day exists, etc.
                var workoutDay = await _workoutRepository.GetWorkoutDayByIdAsync(workoutDayId);
                if (workoutDay == null)
                {
                    return (false, null, "Workout day not found.");
                }

                var exercise = await _workoutRepository.GetExerciseByIdAsync(model.ExerciseId);
                if (exercise == null)
                {
                    return (false, null, "Exercise not found.");
                }

                var plannedExercise = new PlannedExercises
                {
                    WorkoutDayId = workoutDayId,
                    ExerciseId = model.ExerciseId,
                    Sets = model.Sets,
                    Reps = model.Reps,
                    Weight = model.Weight,
                    Notes = model.Notes
                };

                // Delegate the actual database save operation to the data layer
                await _workoutRepository.AddPlannedExerciseAsync(plannedExercise);

                return (true, exercise.Name, null);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error in AddExerciseToDayAsync: " + ex.Message);
                return (false, null, "An error occurred while adding the exercise.");
            }
        }


        public async Task<WorkoutRoutines?> GetWorkoutRoutineByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }
            return await _workoutRepository.GetWorkoutRoutineByIdAsync(id);
        }

        public async Task<List<WorkoutRoutines>> GetWorkoutRoutinesByUserAsync(string id)
        {
            return await _workoutRepository.GetWorkoutRoutinesByUserAsync(id);
        }

        public async Task CompleteRoutineAsync(int id)
        {
            var workoutRoutine = await _workoutRepository.GetWorkoutRoutineByIdAsync(id);
            if (workoutRoutine != null && workoutRoutine.Status == "Draft")
            {
                workoutRoutine.Status = "Completed";
                await _workoutRepository.UpdateWorkoutRoutineAsync(workoutRoutine);
            }
        }

        public async Task<List<Exercises>> GetExercisesAsync()
        {
            return await _workoutRepository.GetExercisesAsync();
        }

        public async Task UpdatePlannedExerciseAsync(PlannedExercises plannedExercise)
        {
            await _workoutRepository.UpdatePlannedExerciseAsync(plannedExercise);
        }

        public async Task<PlannedExercises> GetPlannedExerciseByIdAsync(int plannedExercisesId)
        {
            return await _workoutRepository.GetPlannedExerciseByIdAsync(plannedExercisesId);
        }


        public async Task<bool> RemovePlannedExerciseAsync(int plannedExercisesId)
        {
            return await _workoutRepository.RemovePlannedExerciseAsync(plannedExercisesId);
        }

        public async Task AddPlannedExerciseAsync(PlannedExercises plannedExercise)
        {
            await _workoutRepository.AddPlannedExerciseAsync(plannedExercise);
        }


    }
}
