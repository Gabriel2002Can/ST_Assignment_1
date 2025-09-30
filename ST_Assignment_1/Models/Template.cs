using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ST_Assignment_1.Models
{
    public enum TemplateFrequency { Daily, EveryOtherDay, Weekly, Custom }

    public class Template
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TemplateFrequency Frequency { get; set; }
        public ICollection<ExerciseTemplateItem> Items { get; set; }
    }

    public class ExerciseTemplateItem
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public int ExerciseId { get; set; }
        // Ignore navigation in JSON (client only provides ExerciseId) and exclude from model binding validation
        [JsonIgnore]
        [BindNever]
        public Exercise? Exercise { get; set; }
        public int Order { get; set; }
        public int TargetSets { get; set; }
        public string TargetReps { get; set; }
        public int TargetRestSeconds { get; set; }
        public string Notes { get; set; }
    }
}