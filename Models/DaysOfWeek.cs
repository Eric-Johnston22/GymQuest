using System.ComponentModel.DataAnnotations;

namespace GymQuest.Models
{
    public class DaysOfWeek
    {
        [Key]
        public int DayId { get; set; }
        public string? DayName { get; set; }
    }
}
