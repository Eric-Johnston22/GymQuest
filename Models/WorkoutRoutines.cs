using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace GymQuest.Models
{
    public class WorkoutRoutines
    {
        [Key]
        public int WorkoutRoutineId { get; set; }
        [ForeignKey("Id")]
        public string? UserId { get; set; }
        public string? RoutineName { get; set; }
        public int CycleDays { get; set; }
        public bool IsCycle { get; set; }
        public DateTime CreatedAt { get; set; }

        public User? User { get; set; } // Navigation property
    }
}
