namespace GymQuest.Models.ViewModels
{
    public class ViewRoutineViewModel
    {
        public int WorkoutRoutineId { get; set; }
        public string? RoutineName { get; set; }
        public int CycleDays { get; set; }
        public bool IsCycle { get; set; }
        public List<ViewRoutineDayViewModel> WorkoutDays { get; set; } = new List<ViewRoutineDayViewModel>();
    }

    public class ViewRoutineDayViewModel
    {
        public string? DayName { get; set; }
        public List<ViewRoutineExerciseViewModel> Exercises { get; set; } = new List<ViewRoutineExerciseViewModel>();
    }

    public class ViewRoutineExerciseViewModel
    {
        public string? ExerciseName { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public decimal Weight { get; set; }
        public string? Notes { get; set; }
    }

}
