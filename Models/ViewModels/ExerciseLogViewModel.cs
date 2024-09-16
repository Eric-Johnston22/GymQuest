namespace GymQuest.Models.ViewModels
{
    public class ExerciseLogViewModel
    {
        public int PlannedExercisesId { get; set; }
        public string ExerciseName { get; set; }
        public int SetNumber { get; set; }
        public int Sets { get; set; }
        public int GoalReps { get; set; }
        public int RepsCompleted { get; set; }
        public decimal Weight { get; set; }
        public bool IsSuccessful { get; set; } // Checkbox for successful completion
        public int? FailedRep { get; set; } // Optional: to log the failed rep number
        public string? Notes { get; set; }
    }
}
