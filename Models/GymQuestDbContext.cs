﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

            builder.Entity<ExerciseLogs>()
                .Property(e => e.Weight)
                .HasPrecision(18, 2);
        }
    }
}