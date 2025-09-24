using System;

namespace ST_Assignment_1.Models
{
    public class SetRecord
    {
        public int Id { get; set; }
        public int SessionExerciseId { get; set; }
        public SessionExercise SessionExercise { get; set; }
        public int SetNumber { get; set; }
        public string PlannedRepsOrDuration { get; set; }
        public string ActualRepsOrDuration { get; set; }
        public int RestAfterSetSeconds { get; set; }
        public DateTime Timestamp { get; set; }
    }
}