namespace GymQuest.Models.ViewModels
{
    public class StartRoutineViewModel
    {
        public int WorkoutRoutineId { get; set; }
        public string RoutineName { get; set; }
        public string DayName { get; set; }
        public List<ExerciseLogViewModel> PlannedExercises { get; set; } = new List<ExerciseLogViewModel>();
    }

}
