using GymQuest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymQuest.DataAccess
{
    public class UserDAO
    {
        private readonly GymQuestDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserDAO(GymQuestDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

    }
}
