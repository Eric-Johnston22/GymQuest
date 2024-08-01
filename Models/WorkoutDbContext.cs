using Microsoft.EntityFrameworkCore;

namespace GymQuest.Models
{
    public class WorkoutDbContext : DbContext
    {
        public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : base(options) 
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
