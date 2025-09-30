using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST_Assignment_1.Data;
using ST_Assignment_1.Models;

namespace ST_Assignment_1.Controllers
{
    /// <summary>
    /// CRUD operations for exercises that can be included in workout templates.
    /// </summary>
    [ApiController]
    [Route("api/exercises")]
    public class ExercisesController : ControllerBase
    {
        private readonly WorkoutJournalDbContext _db;
        public ExercisesController(WorkoutJournalDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Returns all exercises.
        /// </summary>
        /// <remarks>
        /// Use this to populate dropdowns or lists when building a template.
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetAll()
        {
            return await _db.Exercises.ToListAsync();
        }

        /// <summary>
        /// Returns a single exercise by id.
        /// </summary>
        /// <param name="id">Exercise identifier</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Exercise>> GetById(int id)
        {
            var exercise = await _db.Exercises.FindAsync(id);
            if (exercise == null) return NotFound();
            return exercise;
        }

        /// <summary>
        /// Creates a new exercise.
        /// </summary>
        /// <param name="exercise">Exercise payload (Id is ignored).</param>
        /// <returns>The created exercise including generated Id.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Exercise>> Create(Exercise exercise)
        {
            _db.Exercises.Add(exercise);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = exercise.Id }, exercise);
        }

        /// <summary>
        /// Updates an existing exercise.
        /// </summary>
        /// <param name="id">Exercise identifier</param>
        /// <param name="updated">Updated fields (all required)</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, Exercise updated)
        {
            var exercise = await _db.Exercises.FindAsync(id);
            if (exercise == null) return NotFound();
            exercise.Name = updated.Name;
            exercise.Category = updated.Category;
            exercise.Description = updated.Description;
            exercise.DefaultSets = updated.DefaultSets;
            exercise.DefaultReps = updated.DefaultReps;
            exercise.DefaultRestSeconds = updated.DefaultRestSeconds;
            exercise.Difficulty = updated.Difficulty;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes an exercise.
        /// </summary>
        /// <param name="id">Exercise identifier</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var exercise = await _db.Exercises.FindAsync(id);
            if (exercise == null) return NotFound();
            _db.Exercises.Remove(exercise);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
