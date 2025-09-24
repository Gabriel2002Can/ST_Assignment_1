using System;
using System.Collections.Generic;

namespace ST_Assignment_1.Models
{
    public class WorkoutSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int? CalendarEntryId { get; set; }
        public CalendarEntry CalendarEntry { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ICollection<SessionExercise> SessionItems { get; set; }
        public string Notes { get; set; }
    }
}