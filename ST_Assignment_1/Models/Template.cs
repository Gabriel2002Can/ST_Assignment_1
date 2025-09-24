using System;
using System.Collections.Generic;

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
}