using System.ComponentModel.DataAnnotations;

namespace GymQuest.Models.ViewModels
{
    public class AssignExercisesViewModel
    {
        public int WorkoutRoutineId { get; set; } // Hidden field to track the routine

        public List<WorkoutDayExercisesViewModel> WorkoutDays { get; set; } = new List<WorkoutDayExercisesViewModel>();

        public class WorkoutDayExercisesViewModel
        {
            public int WorkoutDayId { get; set; } // Id for the Workout Day

            public string DayName { get; set; } // Display the day name (e.g., Monday)

            public List<PlannedExerciseViewModel> PlannedExercises { get; set; } = new List<PlannedExerciseViewModel>();

            public class PlannedExerciseViewModel
            {
                [Required]
                public int ExerciseId { get; set; } // Id of the exercise from the list

                [Required]
                public int Sets { get; set; } // Number of sets

                [Required]
                public int Reps { get; set; } // Number of reps

                public decimal Weight { get; set; } // Weight used in the exercise

                public string? Notes { get; set; } // Optional notes for the exercise
            }
        }
    }
}
