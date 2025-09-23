using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST_Assignment_1.Data;
using ST_Assignment_1.Models;

namespace ST_Assignment_1.Controllers
{
    [ApiController]
    [Route("api/sessions")]
    public class SessionsController : ControllerBase
    {
        private readonly WorkoutJournalDbContext _db;
        public SessionsController(WorkoutJournalDbContext db)
        {
            _db = db;
        }

        // POST /api/sessions/start
        [HttpPost("start")]
        public async Task<ActionResult<WorkoutSession>> StartSession([FromBody] StartSessionRequest req)
        {
            var entry = await _db.CalendarEntries
                .Include(e => e.CustomItems)
                .FirstOrDefaultAsync(e => e.Id == req.CalendarEntryId);
            if (entry == null) return NotFound("Calendar entry not found");
            var session = new WorkoutSession
            {
                Id = Guid.NewGuid(),
                UserId = req.UserId,
                CalendarEntryId = req.CalendarEntryId,
                StartTime = DateTime.UtcNow,
                SessionItems = new List<SessionExercise>(),
                Notes = ""
            };
            // Snapshot planned targets from CustomItems
            foreach (var item in entry.CustomItems)
            {
                session.SessionItems.Add(new SessionExercise
                {
                    Id = Guid.NewGuid(),
                    ExerciseId = item.ExerciseId,
                    Order = item.Order,
                    TargetSets = item.TargetSets,
                    TargetReps = item.TargetReps,
                    TargetRestSeconds = item.TargetRestSeconds,
                    CompletedSets = 0,
                    Sets = new List<SetRecord>()
                });
            }
            _db.WorkoutSessions.Add(session);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSession), new { id = session.Id }, session);
        }

        // PATCH /api/sessions/{id}/exercise/{exerciseId}/set
        [HttpPatch("{id}/exercise/{exerciseId}/set")]
        public async Task<IActionResult> RecordSet(Guid id, Guid exerciseId, [FromBody] RecordSetRequest req)
        {
            var sessionExercise = await _db.SessionExercises.Include(se => se.Sets)
                .FirstOrDefaultAsync(se => se.SessionId == id && se.Id == exerciseId);
            if (sessionExercise == null) return NotFound();
            var setRecord = new SetRecord
            {
                Id = Guid.NewGuid(),
                SessionExerciseId = sessionExercise.Id,
                SetNumber = req.SetNumber,
                PlannedRepsOrDuration = sessionExercise.TargetReps,
                ActualRepsOrDuration = req.ActualRepsOrDuration,
                RestAfterSetSeconds = req.RestAfterSetSeconds,
                Timestamp = DateTime.UtcNow
            };
            sessionExercise.Sets.Add(setRecord);
            sessionExercise.CompletedSets = sessionExercise.Sets.Count;
            await _db.SaveChangesAsync();
            return Ok(setRecord);
        }

        // PUT /api/sessions/{id}/end
        [HttpPut("{id}/end")]
        public async Task<IActionResult> EndSession(Guid id)
        {
            var session = await _db.WorkoutSessions.FindAsync(id);
            if (session == null) return NotFound();
            session.EndTime = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // GET /api/sessions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutSession>> GetSession(Guid id)
        {
            var session = await _db.WorkoutSessions
                .Include(s => s.SessionItems)
                .ThenInclude(se => se.Sets)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (session == null) return NotFound();
            return session;
        }

        // GET /api/sessions?userId=&start=&end=
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutSession>>> GetHistory([FromQuery] Guid userId, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            var query = _db.WorkoutSessions
                .Include(s => s.SessionItems)
                .ThenInclude(se => se.Sets)
                .Where(s => s.UserId == userId);
            if (start.HasValue) query = query.Where(s => s.StartTime >= start);
            if (end.HasValue) query = query.Where(s => s.StartTime <= end);
            return await query.ToListAsync();
        }

        public class StartSessionRequest
        {
            public Guid UserId { get; set; }
            public Guid CalendarEntryId { get; set; }
        }

        public class RecordSetRequest
        {
            public int SetNumber { get; set; }
            public string ActualRepsOrDuration { get; set; }
            public int RestAfterSetSeconds { get; set; }
        }
    }
}
