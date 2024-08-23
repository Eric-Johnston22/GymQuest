using GymQuest.Data;
using GymQuest.Models;
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
            // You can add any additional business logic here before saving
            await _exerciseTrackingRepository.LogExerciseAsync(exerciseLog);
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

    }
}
