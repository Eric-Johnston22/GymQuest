using GymQuest.Data;
using GymQuest.Models;
using GymQuest.Models.ViewModels;
using System.Security.Claims;
using static GymQuest.Models.ViewModels.CreateRoutineViewModel;

namespace GymQuest.Services
{
    public class WorkoutService
    {
        private readonly WorkoutRepository _workoutRepository;
        private readonly UserService _userService;

        public WorkoutService(WorkoutRepository workoutRepository, UserService userService)
        {
            _workoutRepository = workoutRepository;
            _userService = userService;
        }

        public async Task<int> CreateRoutineAsync(CreateRoutineViewModel model, ClaimsPrincipal user)
        {
            string userId = _userService.GetUserId(user);
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
    }
}
