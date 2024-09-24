namespace GymQuest.Models.ViewModels
{
    // These classes are for creating a new routine
    public class CreateRoutineViewModel
    {
        public int WorkoutRoutineId { get; set; } // This will remain 0 for a new routine
        public string? RoutineName { get; set; }
        public int CycleDays { get; set; }
        public bool IsCycle { get; set; }
        public List<CreateRoutineDayViewModel> WorkoutDays { get; set; } = new List<CreateRoutineDayViewModel>();
    }

    public class CreateRoutineDayViewModel
    {
        public string? DayName { get; set; } // Dropdown or text input for day name
        public string? WorkoutType { get; set; }
        public int WorkoutDayId { get; set; }
        public int DayId { get; set; }
        public List<CreateRoutineExerciseViewModel> Exercises { get; set; } = new List<CreateRoutineExerciseViewModel>();
    }

    public class CreateRoutineExerciseViewModel
    {
        public string? ExerciseName { get; set; }
        public int PlannedExerciseId { get; set; }
        public int WorkoutDayId { get; set; }
        public int ExerciseId { get; set; } 
        public int Sets { get; set; }
        public int Reps { get; set; }
        public decimal Weight { get; set; }
        public string? Notes { get; set; }
    }

    // These classes are for viewing an existing routine
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
        public int WorkoutDayId { get; set; }
        public string? WorkoutType { get; set; }
        public int DayId { get; set; }
        public List<ViewRoutineExerciseViewModel> Exercises { get; set; } = new List<ViewRoutineExerciseViewModel>();
    }

    public class ViewRoutineExerciseViewModel
    {
        public string? ExerciseName { get; set; }
        public int ExerciseId { get; set; }
        public int PlannedExerciseId { get; set; }
        public int WorkoutDayId { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public decimal Weight { get; set; }
        public string? Notes { get; set; }
    }
}
