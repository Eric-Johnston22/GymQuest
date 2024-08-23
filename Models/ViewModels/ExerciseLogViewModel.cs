namespace GymQuest.Models.ViewModels
{
    public class ExerciseLogViewModel
    {
        public int PlannedExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public int SetNumber { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public decimal Weight { get; set; }
        public bool IsSuccessful { get; set; } // Checkbox for successful completion
        public int? FailedRep { get; set; } // Optional: to log the failed rep number
    }
}
