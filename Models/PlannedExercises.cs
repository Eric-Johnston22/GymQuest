using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymQuest.Models
{
    public class PlannedExercises
    {
        [Key]
        public int PlannedExercisesId { get; set; }
        [ForeignKey("WorkoutDayId")]
        public int WorkoutDayId { get; set; }
        [ForeignKey("ExerciseId")]
        public int ExerciseId { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public decimal Weight { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public WorkoutDays? WorkoutDays { get; set; }
        public Exercises? Exercises { get; set; }
    }
}
