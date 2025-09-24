using System;
using System.Collections.Generic;

namespace ST_Assignment_1.Models
{
    public enum ExerciseCategory { Push, Pull, Legs, Core, Conditioning }
    public enum ExerciseDifficulty { Beginner, Intermediate, Advanced }

    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ExerciseCategory Category { get; set; }
        public string Description { get; set; }
        public int DefaultSets { get; set; }
        public string DefaultReps { get; set; }
        public int DefaultRestSeconds { get; set; }
        public ExerciseDifficulty Difficulty { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}