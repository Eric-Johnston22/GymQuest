using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GymQuest.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
