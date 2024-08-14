using GymQuest.Models;
using GymQuest.Models.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GymQuest.Data
{
    public class WorkoutRepository
    {
        private readonly GymQuestDbContext _context;

        public WorkoutRepository( GymQuestDbContext context)
        { 
            _context = context;
        }

        
    }
}
