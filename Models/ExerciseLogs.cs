using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymQuest.Models
{
    public class ExerciseLogs
    {
        [Key]
        public int LogId { get; set; }
        [ForeignKey("Id")]
        public string? UserId { get; set; }
        [ForeignKey("PlannedExerciseId")]
        public int PlannedExerciseId { get; set; }
        public DateTime Date { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public decimal Weight { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public virtual PlannedExercises? PlannedExercises { get; set; }
        public virtual User? User { get; set; }
    }
}
