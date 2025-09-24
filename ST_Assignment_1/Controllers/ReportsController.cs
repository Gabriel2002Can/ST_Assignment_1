using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST_Assignment_1.Data;
using ST_Assignment_1.Models;

namespace ST_Assignment_1.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly WorkoutJournalDbContext _db;
        public ReportsController(WorkoutJournalDbContext db)
        {
            _db = db;
        }

        // GET /api/reports/progress?exerciseId=&start=&end=
        [HttpGet("progress")]
        public async Task<ActionResult<ProgressReport>> GetProgress([FromQuery] int exerciseId, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            var sets = await _db.SetRecords
                .Include(sr => sr.SessionExercise)
                .Where(sr => sr.SessionExerciseId == exerciseId)
                .Where(sr => !start.HasValue || sr.Timestamp >= start)
                .Where(sr => !end.HasValue || sr.Timestamp <= end)
                .ToListAsync();
            if (!sets.Any()) return Ok(new ProgressReport());
            var totalVolume = sets.Sum(sr => int.TryParse(sr.ActualRepsOrDuration, out var reps) ? reps : 0);
            var bestReps = sets.Max(sr => int.TryParse(sr.ActualRepsOrDuration, out var reps) ? reps : 0);
            var frequency = sets.Select(sr => sr.Timestamp.Date).Distinct().Count();
            return Ok(new ProgressReport
            {
                TotalVolume = totalVolume,
                BestReps = bestReps,
                Frequency = frequency
            });
        }

        public class ProgressReport
        {
            public int TotalVolume { get; set; }
            public int BestReps { get; set; }
            public int Frequency { get; set; }
        }
    }
}
