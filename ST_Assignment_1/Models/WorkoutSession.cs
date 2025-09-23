using System;
using System.Collections.Generic;

namespace ST_Assignment_1.Models
{
    public class WorkoutSession
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid? CalendarEntryId { get; set; }
        public CalendarEntry CalendarEntry { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ICollection<SessionExercise> SessionItems { get; set; }
        public string Notes { get; set; }
    }
}