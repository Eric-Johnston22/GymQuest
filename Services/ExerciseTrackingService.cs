using GymQuest.Data;
using GymQuest.Models;
using GymQuest.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.Services
{
    public class ExerciseTrackingService
    {
        private readonly ExerciseTrackingRepository _exerciseTrackingRepository;

        public ExerciseTrackingService(ExerciseTrackingRepository exerciseTrackingRepository)
        {
            _exerciseTrackingRepository = exerciseTrackingRepository;
        }

        public async Task LogExerciseAsync(ExerciseLogs exerciseLog)
        {
            try
            {
                await _exerciseTrackingRepository.LogExerciseAsync(exerciseLog);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LogExerciseAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<WorkoutSummaryViewModel> GetWorkoutSummaryAsync(string userId, int routineId)
        {
            var logs = await _exerciseTrackingRepository.GetExerciseLogsByRoutineAsync(userId, routineId);

            var workoutSummary = new WorkoutSummaryViewModel
            {
                RoutineName = logs.FirstOrDefault()?.PlannedExercises?.WorkoutDays?.WorkoutRoutine?.RoutineName ?? "Unknown Routine",
                WorkoutDate = DateTime.Now, // or get it from logs if needed
                Exercises = logs.GroupBy(log => log.PlannedExercises.Exercises.Name)
                    .Select(group => new WorkoutSummaryViewModel.ExerciseSummaryViewModel
                    {
                        ExerciseName = group.Key,
                        TotalSets = group.Count(),
                        TotalReps = group.Sum(g => g.RepsCompleted),
                        MaxWeight = group.Max(g => g.Weight),
                        IsPR = CalculateIfPR(group, userId), // Function to calculate if PR was achieved
                        Sets = group.Select(g => new WorkoutSummaryViewModel.SetDetailViewModel
                        {
                            SetNumber = g.SetNumber,
                            RepsCompleted = g.RepsCompleted,
                            Weight = g.Weight,
                            Notes = g.Notes
                        }).ToList()
                    }).ToList()
            };

            return workoutSummary;
        }

        // Example PR calculation method
        private bool CalculateIfPR(IEnumerable<ExerciseLogs> logs, string userId)
        {
            // Logic to determine if a PR was achieved
            // This could involve comparing the current max weight or reps to historical data
            return false; // Placeholder logic
        }


        public async Task<ExerciseLogs?> GetExerciseLogByIdAsync(int logId)
        {
            return await _exerciseTrackingRepository.GetExerciseLogByIdAsync(logId);
        }

        public async Task<int> GetOrCreateExerciseAsync(string exerciseName)
        {
            var exercise = await _exerciseTrackingRepository.GetExerciseByNameAsync(exerciseName);
            if (exercise == null)
            {
                exercise = new Exercises { Name = exerciseName };
                await _exerciseTrackingRepository.AddExerciseAsync(exercise);
            }
            return exercise.ExerciseId;
        }

        public async Task AddPlannedExerciseAsync(PlannedExercises exercise)
        {
            await _exerciseTrackingRepository.AddPlannedExerciseAsync(exercise);
        }

        public async Task SetCurrentRoutineAsync(string userId, int routineId)
        {
            await _exerciseTrackingRepository.SetCurrentRoutine(userId, routineId);
        }

    }
}
