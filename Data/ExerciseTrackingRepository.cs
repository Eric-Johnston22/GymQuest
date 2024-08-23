using GymQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.Data
{
    public class ExerciseTrackingRepository
    {
        private readonly GymQuestDbContext _context;

        public ExerciseTrackingRepository(GymQuestDbContext context)
        {
            _context = context;
        }

        public async Task LogExerciseAsync(ExerciseLogs exerciseLog)
        {
            _context.ExerciseLogs.Add(exerciseLog);
            await _context.SaveChangesAsync();
        }

        public async Task<ExerciseLogs?> GetExerciseLogByIdAsync(int logId)
        {
            return await _context.ExerciseLogs
                .Include(log => log.PlannedExercises)
                .Include(log => log.User)
                .FirstOrDefaultAsync(log => log.LogId == logId);
        }

        public async Task<Exercises?> GetExerciseByNameAsync(string exerciseName)
        {
            return await _context.Exercises.FirstOrDefaultAsync(e => e.Name == exerciseName);
        }

        public async Task AddExerciseAsync(Exercises exercise)
        {
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
        }

        public async Task AddPlannedExerciseAsync(PlannedExercises exercise)
        {
            _context.PlannedExercises.Add(exercise);
            await _context.SaveChangesAsync();
        }
    }
}
