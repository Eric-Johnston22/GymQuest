using GymQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.DataAccess
{
    public class UserDAO
    {
        private readonly GymQuestDbContext _context;

        public UserDAO(GymQuestDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetFirstAsync()
        {
            return await _context.Users.FirstAsync();
        }
    }
}
