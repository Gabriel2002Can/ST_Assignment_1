using System;
using System.Collections.Generic;

namespace ST_Assignment_1.Models
{
    public enum CalendarStatus { Scheduled, Skipped, Completed }

    public class CalendarEntry
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public Guid? TemplateId { get; set; }
        public Template Template { get; set; }
        public ICollection<ExerciseTemplateItem> CustomItems { get; set; }
        public CalendarStatus Status { get; set; }
    }
}