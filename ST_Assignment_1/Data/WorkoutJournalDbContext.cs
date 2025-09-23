using Microsoft.EntityFrameworkCore;
using ST_Assignment_1.Models;
using System;
using System.Collections.Generic;

namespace ST_Assignment_1.Data
{
    public class WorkoutJournalDbContext : DbContext
    {
        public WorkoutJournalDbContext(DbContextOptions<WorkoutJournalDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<ExerciseTemplateItem> TemplateItems { get; set; }
        public DbSet<CalendarEntry> CalendarEntries { get; set; }
        public DbSet<WorkoutSession> WorkoutSessions { get; set; }
        public DbSet<SessionExercise> SessionExercises { get; set; }
        public DbSet<SetRecord> SetRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>()
                .HasMany(u => u.CalendarEntries)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);
            modelBuilder.Entity<User>()
                .HasMany(u => u.WorkoutSessions)
                .WithOne(ws => ws.User)
                .HasForeignKey(ws => ws.UserId);

            // Template & Items
            modelBuilder.Entity<Template>()
                .HasMany(t => t.Items)
                .WithOne()
                .HasForeignKey(i => i.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // CalendarEntry
            modelBuilder.Entity<CalendarEntry>()
                .HasOne(c => c.Template)
                .WithMany()
                .HasForeignKey(c => c.TemplateId);
            modelBuilder.Entity<CalendarEntry>()
                .HasMany(c => c.CustomItems)
                .WithOne()
                .HasForeignKey(i => i.Id);

            // WorkoutSession & SessionExercise
            modelBuilder.Entity<WorkoutSession>()
                .HasMany(ws => ws.SessionItems)
                .WithOne(se => se.Session)
                .HasForeignKey(se => se.SessionId);

            // SessionExercise & SetRecord
            modelBuilder.Entity<SessionExercise>()
                .HasMany(se => se.Sets)
                .WithOne(sr => sr.SessionExercise)
                .HasForeignKey(sr => sr.SessionExerciseId);

            // ExerciseTemplateItem
            modelBuilder.Entity<ExerciseTemplateItem>()
                .HasOne(i => i.Exercise)
                .WithMany()
                .HasForeignKey(i => i.ExerciseId);

            // SessionExercise
            modelBuilder.Entity<SessionExercise>()
                .HasOne(se => se.Exercise)
                .WithMany()
                .HasForeignKey(se => se.ExerciseId);

            // Seed curated calisthenics exercises
            modelBuilder.Entity<Exercise>().HasData(
                new Exercise { Id = Guid.NewGuid(), Name = "Push-up", Category = ExerciseCategory.Push, Description = "Standard push-up. Keep body aligned.", DefaultSets = 3, DefaultReps = "8-15", DefaultRestSeconds = 75, Difficulty = ExerciseDifficulty.Beginner, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Incline Push-up", Category = ExerciseCategory.Push, Description = "Incline push-up.", DefaultSets = 3, DefaultReps = "8-20", DefaultRestSeconds = 60, Difficulty = ExerciseDifficulty.Beginner, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Diamond Push-up", Category = ExerciseCategory.Push, Description = "Diamond push-up.", DefaultSets = 3, DefaultReps = "6-12", DefaultRestSeconds = 90, Difficulty = ExerciseDifficulty.Intermediate, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Pike Push-up", Category = ExerciseCategory.Push, Description = "Pike push-up.", DefaultSets = 3, DefaultReps = "6-12", DefaultRestSeconds = 105, Difficulty = ExerciseDifficulty.Intermediate, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Pull-up", Category = ExerciseCategory.Pull, Description = "Pull-up.", DefaultSets = 4, DefaultReps = "3-10", DefaultRestSeconds = 135, Difficulty = ExerciseDifficulty.Intermediate, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Chin-up", Category = ExerciseCategory.Pull, Description = "Chin-up.", DefaultSets = 4, DefaultReps = "3-10", DefaultRestSeconds = 135, Difficulty = ExerciseDifficulty.Intermediate, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Bodyweight Row (Australian Pull-up)", Category = ExerciseCategory.Pull, Description = "Bodyweight row.", DefaultSets = 3, DefaultReps = "8-15", DefaultRestSeconds = 75, Difficulty = ExerciseDifficulty.Beginner, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Dips (parallel bar)", Category = ExerciseCategory.Push, Description = "Parallel bar dips.", DefaultSets = 3, DefaultReps = "6-15", DefaultRestSeconds = 105, Difficulty = ExerciseDifficulty.Intermediate, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Squat (bodyweight)", Category = ExerciseCategory.Legs, Description = "Bodyweight squat.", DefaultSets = 3, DefaultReps = "12-20", DefaultRestSeconds = 60, Difficulty = ExerciseDifficulty.Beginner, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Jump Squat", Category = ExerciseCategory.Legs, Description = "Jump squat.", DefaultSets = 3, DefaultReps = "8-15", DefaultRestSeconds = 75, Difficulty = ExerciseDifficulty.Intermediate, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Pistol Squat (assisted)", Category = ExerciseCategory.Legs, Description = "Assisted pistol squat.", DefaultSets = 4, DefaultReps = "3-8", DefaultRestSeconds = 135, Difficulty = ExerciseDifficulty.Advanced, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Lunges (walking/static)", Category = ExerciseCategory.Legs, Description = "Lunges.", DefaultSets = 3, DefaultReps = "8-15", DefaultRestSeconds = 75, Difficulty = ExerciseDifficulty.Beginner, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Plank", Category = ExerciseCategory.Core, Description = "Plank.", DefaultSets = 3, DefaultReps = "30-90s", DefaultRestSeconds = 60, Difficulty = ExerciseDifficulty.Beginner, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Hanging Leg Raise", Category = ExerciseCategory.Core, Description = "Hanging leg raise.", DefaultSets = 3, DefaultReps = "8-15", DefaultRestSeconds = 75, Difficulty = ExerciseDifficulty.Intermediate, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Mountain Climbers", Category = ExerciseCategory.Conditioning, Description = "Mountain climbers.", DefaultSets = 3, DefaultReps = "30s", DefaultRestSeconds = 45, Difficulty = ExerciseDifficulty.Beginner, CreatedAt = DateTime.UtcNow },
                new Exercise { Id = Guid.NewGuid(), Name = "Burpee", Category = ExerciseCategory.Conditioning, Description = "Burpee.", DefaultSets = 3, DefaultReps = "6-15", DefaultRestSeconds = 90, Difficulty = ExerciseDifficulty.Intermediate, CreatedAt = DateTime.UtcNow }
            );
        }
    }
}