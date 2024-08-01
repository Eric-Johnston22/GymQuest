using GymQuest.Models;

namespace GymQuest.DataAccess
{
    public class UserDAO
    {
        private readonly WorkoutDbContext _context;

        public UserDAO(WorkoutDbContext context)
        {
            _context = context;
        }

        public User GetFirst()
        {
            return _context.Users.First();
        }
    }
}
