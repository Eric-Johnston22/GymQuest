using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymQuest.Models
{
    public class WorkoutDays
    {
        [Key]
        public int WorkoutDayId { get; set; }
        [ForeignKey("WorkoutRoutineId")]
        public int WorkoutRoutineId { get; set; }
        public int DayInCycle { get; set; }
        [ForeignKey("DayId")]
        public int DayId { get; set; }
        public string? WorkoutType { get; set; }

        public virtual DaysOfWeek? DaysOfWeek { get; set; } // Navigation property

        // Navigation property to represent the collection of exercises planned for this day
        public virtual ICollection<PlannedExercises> PlannedExercises { get; set; } = new List<PlannedExercises>();
    }
}
