using System;
using System.Collections.Generic;

namespace ST_Assignment_1.Models
{
    public class ExerciseTemplateItem
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public int Order { get; set; }
        public int TargetSets { get; set; }
        public string TargetReps { get; set; }
        public int TargetRestSeconds { get; set; }
        public string Notes { get; set; }
    }
}