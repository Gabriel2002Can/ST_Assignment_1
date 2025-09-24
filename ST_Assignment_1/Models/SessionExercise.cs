using System;
using System.Collections.Generic;

namespace ST_Assignment_1.Models
{
    public class SessionExercise
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public WorkoutSession Session { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public int Order { get; set; }
        public int TargetSets { get; set; }
        public string TargetReps { get; set; }
        public int TargetRestSeconds { get; set; }
        public int CompletedSets { get; set; }
        public ICollection<SetRecord> Sets { get; set; }
    }
}