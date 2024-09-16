using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GymQuest.Models
{
    public class GymQuestDbContext : IdentityDbContext<User>
    {
        public GymQuestDbContext(DbContextOptions<GymQuestDbContext> options) : base(options) 
        {
        }

        public DbSet<WorkoutRoutines> WorkoutRoutines { get; set; }
        public DbSet<WorkoutDays> WorkoutDays { get; set; }
        public DbSet<PlannedExercises> PlannedExercises { get; set; }
        public DbSet<Exercises> Exercises { get; set; }
        public DbSet<ExerciseLogs> ExerciseLogs { get; set; }
        public DbSet<DaysOfWeek> DaysOfWeek { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PlannedExercises>()
                .Property(e => e.Weight)
            .HasPrecision(18, 2);

            builder.Entity<PlannedExercises>()
                .HasOne(pe => pe.WorkoutDays)
                .WithMany(wd => wd.PlannedExercises)
                .HasForeignKey(pe => pe.WorkoutDayId);

            builder.Entity<PlannedExercises>()
                .HasOne(pe => pe.Exercises)
                .WithMany()
                .HasForeignKey(pe => pe.ExerciseId);

            builder.Entity<ExerciseLogs>()
                .Property(e => e.Weight)
                .HasPrecision(18, 2);

            builder.Entity<WorkoutDays>()
                .HasOne(wd => wd.DaysOfWeek)
                .WithMany()
                .HasForeignKey(wd => wd.DayId);

            // Configuring the one-to-one relationship between User and WorkoutRoutines
            builder.Entity<User>()
                .HasOne(u => u.CurrentWorkoutRoutine)
                .WithOne(wr => wr.User)
                .HasForeignKey<User>(u => u.CurrentWorkoutRoutineId); // Explicitly specify the foreign key property

            // Example configuration if necessary
            builder.Entity<ExerciseLogs>()
                .Property(e => e.RepsCompleted)
                .HasColumnType("int");
        }
    }
}
