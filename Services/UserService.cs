﻿using GymQuest.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GymQuest.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task SignInUserAsync(User user, bool isPersistant)
        {
            await _signInManager.SignInAsync(user, isPersistant);
        }

        public async Task<SignInResult> PasswordSignInUserAsync(string email, string password, bool rememberMe, bool lockoutOnFailure)
        {
            return await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure);
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

        public string GetUserId(ClaimsPrincipal user)
        {
            string userId = _userManager.GetUserId(user);
            return userId!;
        }
    }
}
