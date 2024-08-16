using System.ComponentModel.DataAnnotations;

namespace GymQuest.Models.ViewModels
{
    public class AddWorkoutDaysViewModel
    {
        public int WorkoutRoutineId { get; set; } // Hidden field to track the routine

        [Required]
        public List<WorkoutDayViewModel> WorkoutDays { get; set; } = new List<WorkoutDayViewModel>();

        public class WorkoutDayViewModel
        {
            [Required]
            public int DayId { get; set; } // Day of the week (e.g., Monday, Tuesday)

            public string WorkoutType { get; set; } // Type of workout (e.g., Push, Pull, Legs)
        }
    }
}
