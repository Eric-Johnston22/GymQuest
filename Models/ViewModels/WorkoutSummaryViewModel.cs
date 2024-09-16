namespace GymQuest.Models.ViewModels
{
    public class WorkoutSummaryViewModel
    {
        public string RoutineName { get; set; } // Name of the workout routine
        public DateTime WorkoutDate { get; set; } // Date of the workout
        public List<ExerciseSummaryViewModel> Exercises { get; set; } = new List<ExerciseSummaryViewModel>(); // Exercises completed

        public class ExerciseSummaryViewModel
        {
            public string ExerciseName { get; set; } // Name of the exercise
            public int TotalSets { get; set; } // Total sets completed
            public int TotalReps { get; set; } // Total reps completed
            public decimal MaxWeight { get; set; } // Maximum weight lifted
            public bool IsPR { get; set; } // Indicates if a Personal Record was achieved
            public List<SetDetailViewModel> Sets { get; set; } = new List<SetDetailViewModel>(); // Detailed info about each set
        }

        public class SetDetailViewModel
        {
            public int SetNumber { get; set; }
            public int RepsCompleted { get; set; }
            public decimal Weight { get; set; }
            public string? Notes { get; set; } // Optional notes for the set
        }
    }

}
