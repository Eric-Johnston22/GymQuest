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

        public async Task<WorkoutRoutines?> GetWorkoutRoutineByIdAsync(int id)
        {
            return await _workoutRepository.GetWorkoutRoutineByIdAsync(id);
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
    }
}
