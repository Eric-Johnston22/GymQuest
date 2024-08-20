using GymQuest.Models;
using GymQuest.Models.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.Data
{
    public class WorkoutRepository
    {
        private readonly GymQuestDbContext _context;

        public WorkoutRepository( GymQuestDbContext context)
        { 
            _context = context;
        }

        public async Task<int> CreateWorkoutRoutineAsync(WorkoutRoutines routine)
        {
            _context.WorkoutRoutines.Add(routine); // Add new instance of entity class
            await _context.SaveChangesAsync(); // Insert into database
            return routine.WorkoutRoutineId; // Return ID for controller redirect
        }

        public async Task<WorkoutRoutines?> GetWorkoutRoutineByIdAsync(int id)
        {
            return await _context.WorkoutRoutines
                .Include(wr => wr.WorkoutDays)
                .ThenInclude(wd => wd.PlannedExercises)
                .FirstOrDefaultAsync(wr => wr.WorkoutRoutineId == id);
        }

        public async Task UpdateWorkoutRoutineAsync(WorkoutRoutines routine)
        {
            _context.WorkoutRoutines.Update(routine);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Exercises>> GetExercisesAsync()
        {
            return await _context.Exercises.ToListAsync();
        }
    }
}
