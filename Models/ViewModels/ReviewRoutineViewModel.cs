namespace GymQuest.Models.ViewModels
{
    public class ReviewRoutineViewModel
    {
        public WorkoutRoutines? WorkoutRoutine { get; set; } // This can include all the associated days and exercises

        public class WorkoutDaySummaryViewModel
        {
            public string? DayName { get; set; } // Display the day name (e.g., Monday)

            public List<PlannedExerciseSummaryViewModel> PlannedExercises { get; set; } = new List<PlannedExerciseSummaryViewModel>();

            public class PlannedExerciseSummaryViewModel
            {
                public string? ExerciseName { get; set; } // Name of the exercise

                public int Sets { get; set; } // Number of sets

                public int Reps { get; set; } // Number of reps

                public decimal Weight { get; set; } // Weight used in the exercise

                public string? Notes { get; set; } // Optional notes for the exercise
            }
        }
    }
}
