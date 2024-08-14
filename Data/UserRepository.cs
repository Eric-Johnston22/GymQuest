using GymQuest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.DataAccess
{
    public class UserRepository
    {
        private readonly GymQuestDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(GymQuestDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

    }
}
