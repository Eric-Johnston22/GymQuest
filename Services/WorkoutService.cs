using GymQuest.Data;
using GymQuest.Models;
using GymQuest.Models.ViewModels;
using System.Security.Claims;
using static GymQuest.Models.ViewModels.CreateRoutineViewModel;

namespace GymQuest.Services
{
    public class WorkoutService
    {
        private readonly WorkoutRepository _workoutRepository;
        private readonly UserService _userService;

        public WorkoutService(WorkoutRepository workoutRepository, UserService userService)
        {
            _workoutRepository = workoutRepository;
            _userService = userService;
        }

        public async Task CreateRoutineAsync(CreateRoutineViewModel model, ClaimsPrincipal user)
        {
            string userId = _userService.GetUserId(user);
            var workoutRoutine = new WorkoutRoutines
            {
                RoutineName = model.RoutineName,
                CycleDays = model.CycleDays,
                IsCycle = model.IsCycle,
                UserId = userId,
                Status = "Draft" // Set as draft
            };
        }
    }
}
