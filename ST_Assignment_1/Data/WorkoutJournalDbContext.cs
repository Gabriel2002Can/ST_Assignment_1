using Microsoft.EntityFrameworkCore;
using ST_Assignment_1.Models;
using System;

namespace ST_Assignment_1.Data
{
    public class WorkoutJournalDbContext : DbContext
    {
        public WorkoutJournalDbContext(DbContextOptions<WorkoutJournalDbContext> options) : base(options) { }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<ExerciseTemplateItem> TemplateItems { get; set; }
        public DbSet<AppSettings> AppSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Template & Items
            modelBuilder.Entity<Template>()
                .HasMany(t => t.Items)
                .WithOne()
                .HasForeignKey(i => i.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            // ExerciseTemplateItem
            modelBuilder.Entity<ExerciseTemplateItem>()
                .HasOne(i => i.Exercise)
                .WithMany()
                .HasForeignKey(i => i.ExerciseId);

            // AppSettings (singleton row)
            modelBuilder.Entity<AppSettings>().HasData(
                new AppSettings { Id = 1, CurrentTemplateId = 1 }
            );

            // Seed curated calisthenics exercises
            modelBuilder.Entity<Exercise>().HasData(
                new Exercise { Id = 1, Name = "Push-up", Category = ExerciseCategory.Push, Description = "Standard push-up. Keep body aligned.", DefaultSets = 3, DefaultReps = "8-15", DefaultRestSeconds = 75, Difficulty = ExerciseDifficulty.Beginner },
                new Exercise { Id = 2, Name = "Incline Push-up", Category = ExerciseCategory.Push, Description = "Incline push-up.", DefaultSets = 3, DefaultReps = "8-20", DefaultRestSeconds = 60, Difficulty = ExerciseDifficulty.Beginner },
                new Exercise { Id = 3, Name = "Diamond Push-up", Category = ExerciseCategory.Push, Description = "Diamond push-up.", DefaultSets = 3, DefaultReps = "6-12", DefaultRestSeconds = 90, Difficulty = ExerciseDifficulty.Intermediate },
                new Exercise { Id = 4, Name = "Pike Push-up", Category = ExerciseCategory.Push, Description = "Pike push-up.", DefaultSets = 3, DefaultReps = "6-12", DefaultRestSeconds = 105, Difficulty = ExerciseDifficulty.Intermediate },
                new Exercise { Id = 5, Name = "Pull-up", Category = ExerciseCategory.Pull, Description = "Pull-up.", DefaultSets = 4, DefaultReps = "3-10", DefaultRestSeconds = 135, Difficulty = ExerciseDifficulty.Intermediate },
                new Exercise { Id = 6, Name = "Chin-up", Category = ExerciseCategory.Pull, Description = "Chin-up.", DefaultSets = 4, DefaultReps = "3-10", DefaultRestSeconds = 135, Difficulty = ExerciseDifficulty.Intermediate },
                new Exercise { Id = 7, Name = "Bodyweight Row (Australian Pull-up)", Category = ExerciseCategory.Pull, Description = "Bodyweight row.", DefaultSets = 3, DefaultReps = "8-15", DefaultRestSeconds = 75, Difficulty = ExerciseDifficulty.Beginner },
                new Exercise { Id = 8, Name = "Dips (parallel bar)", Category = ExerciseCategory.Push, Description = "Parallel bar dips.", DefaultSets = 3, DefaultReps = "6-15", DefaultRestSeconds = 105, Difficulty = ExerciseDifficulty.Intermediate },
                new Exercise { Id = 9, Name = "Squat (bodyweight)", Category = ExerciseCategory.Legs, Description = "Bodyweight squat.", DefaultSets = 3, DefaultReps = "12-20", DefaultRestSeconds = 60, Difficulty = ExerciseDifficulty.Beginner },
                new Exercise { Id = 10, Name = "Jump Squat", Category = ExerciseCategory.Legs, Description = "Jump squat.", DefaultSets = 3, DefaultReps = "8-15", DefaultRestSeconds = 75, Difficulty = ExerciseDifficulty.Intermediate },
                new Exercise { Id = 11, Name = "Pistol Squat (assisted)", Category = ExerciseCategory.Legs, Description = "Assisted pistol squat.", DefaultSets = 4, DefaultReps = "3-8", DefaultRestSeconds = 135, Difficulty = ExerciseDifficulty.Advanced },
                new Exercise { Id = 12, Name = "Lunges (walking/static)", Category = ExerciseCategory.Legs, Description = "Lunges.", DefaultSets = 3, DefaultReps = "8-15", DefaultRestSeconds = 75, Difficulty = ExerciseDifficulty.Beginner },
                new Exercise { Id = 13, Name = "Plank", Category = ExerciseCategory.Core, Description = "Plank.", DefaultSets = 3, DefaultReps = "30-90s", DefaultRestSeconds = 60, Difficulty = ExerciseDifficulty.Beginner },
                new Exercise { Id = 14, Name = "Hanging Leg Raise", Category = ExerciseCategory.Core, Description = "Hanging leg raise.", DefaultSets = 3, DefaultReps = "8-15", DefaultRestSeconds = 75, Difficulty = ExerciseDifficulty.Intermediate },
                new Exercise { Id = 15, Name = "Mountain Climbers", Category = ExerciseCategory.Conditioning, Description = "Mountain climbers.", DefaultSets = 3, DefaultReps = "30s", DefaultRestSeconds = 45, Difficulty = ExerciseDifficulty.Beginner },
                new Exercise { Id = 16, Name = "Burpee", Category = ExerciseCategory.Conditioning, Description = "Burpee.", DefaultSets = 3, DefaultReps = "6-15", DefaultRestSeconds = 90, Difficulty = ExerciseDifficulty.Intermediate }
            );

            // Seed a template (Full Body Example)
            modelBuilder.Entity<Template>().HasData(
                new Template { Id = 1, Name = "Full Body Example", Description = "A simple full body routine.", Frequency = TemplateFrequency.Daily }
            );

            // Seed template items (linking to seeded exercises)
            modelBuilder.Entity<ExerciseTemplateItem>().HasData(
                new ExerciseTemplateItem { Id = 1, TemplateId = 1, ExerciseId = 1, Order = 1, TargetSets = 3, TargetReps = "10", TargetRestSeconds = 300, Notes = "Push-ups: 10 reps, 3 sets, 5 min rest" },
                new ExerciseTemplateItem { Id = 2, TemplateId = 1, ExerciseId = 2, Order = 2, TargetSets = 3, TargetReps = "15", TargetRestSeconds = 300, Notes = "Squats: 15 reps, 3 sets, 5 min rest" }
            );
        }
    }

    public class AppSettings
    {
        public int Id { get; set; }
        public int CurrentTemplateId { get; set; }
    }
}