using System.ComponentModel.DataAnnotations;

namespace GymQuest.Models
{
    public class Exercises
    {
        [Key]
        public int ExerciseId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
