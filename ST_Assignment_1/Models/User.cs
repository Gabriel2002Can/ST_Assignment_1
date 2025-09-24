using System;
using System.Collections.Generic;

namespace ST_Assignment_1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<CalendarEntry> CalendarEntries { get; set; }
        public ICollection<WorkoutSession> WorkoutSessions { get; set; }
    }
}