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
        public int SetNumber { get; set; } // The specific set number being logged
        public int RepsCompleted { get; set; } // Number of reps completed in this set
        public decimal Weight { get; set; }
        public bool IsSuccessful { get; set; } // Indicates if the set was completed successfully
        public string? Notes { get; set; } // Additional notes, if any

        // Navigation properties
        public virtual PlannedExercises? PlannedExercises { get; set; }
        public virtual User? User { get; set; }
    }
}
