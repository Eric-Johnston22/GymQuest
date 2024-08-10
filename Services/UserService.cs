using GymQuest.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GymQuest.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager; 
        }

        public async Task<string> GetFirstNameAsync(ClaimsPrincipal user)
        {
            var currentUser = await _userManager.GetUserAsync(user);
            
            return currentUser?.FirstName!;
        }

        public async Task<User> GetUserDataAsync(ClaimsPrincipal user)
        {
            var currentUser = await _userManager.GetUserAsync(user);

            return currentUser!;
        }
    }
}
