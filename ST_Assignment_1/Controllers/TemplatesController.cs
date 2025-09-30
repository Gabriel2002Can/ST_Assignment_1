using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST_Assignment_1.Data;
using ST_Assignment_1.Models;

namespace ST_Assignment_1.Controllers
{
    /// <summary>
    /// Manage workout templates composed of ordered exercise items.
    /// </summary>
    [ApiController]
    [Route("api/templates")]
    public class TemplatesController : ControllerBase
    {
        private readonly WorkoutJournalDbContext _db;
        public TemplatesController(WorkoutJournalDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Returns all templates including their items and exercise details.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Template>>> GetAll()
        {
            return await _db.Templates.Include(t => t.Items).ThenInclude(i => i.Exercise).ToListAsync();
        }

        /// <summary>
        /// Returns a single template by id including items and exercises.
        /// </summary>
        /// <param name="id">Template identifier</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Template>> GetById(int id)
        {
            var template = await _db.Templates.Include(t => t.Items).ThenInclude(i => i.Exercise).FirstOrDefaultAsync(t => t.Id == id);
            if (template == null) return NotFound();
            return template;
        }

        /// <summary>
        /// Creates a new template (with optional items).
        /// </summary>
        /// <param name="template">Template payload with Items. Each item must specify ExerciseId.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Template>> Create(Template template)
        {
            _db.Templates.Add(template);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = template.Id }, template);
        }

        /// <summary>
        /// Replaces core template data and its items (full update).
        /// </summary>
        /// <param name="id">Template identifier</param>
        /// <param name="updated">Full replacement payload including desired Items collection.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, Template updated)
        {
            var template = await _db.Templates.Include(t => t.Items).FirstOrDefaultAsync(t => t.Id == id);
            if (template == null) return NotFound();
            template.Name = updated.Name;
            template.Description = updated.Description;
            template.Frequency = updated.Frequency;
            // Replace items
            template.Items.Clear();
            if (updated.Items != null)
            {
                foreach (var item in updated.Items)
                {
                    template.Items.Add(item);
                }
            }
            await _db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a template (items removed by cascade if configured).
        /// </summary>
        /// <param name="id">Template identifier</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var template = await _db.Templates.FindAsync(id);
            if (template == null) return NotFound();
            _db.Templates.Remove(template);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Returns the currently selected template (application setting) including items.
        /// </summary>
        [HttpGet("current")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Template>> GetCurrent()
        {
            var appSettings = await _db.AppSettings.FirstOrDefaultAsync();
            if (appSettings == null) return NotFound();
            var template = await _db.Templates.Include(t => t.Items).ThenInclude(i => i.Exercise).FirstOrDefaultAsync(t => t.Id == appSettings.CurrentTemplateId);
            if (template == null) return NotFound();
            return template;
        }

        /// <summary>
        /// Sets the provided template id as the current template.
        /// </summary>
        /// <param name="id">Template identifier to mark current.</param>
        [HttpPut("current/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetCurrent(int id)
        {
            var template = await _db.Templates.FindAsync(id);
            if (template == null) return NotFound();
            var appSettings = await _db.AppSettings.FirstOrDefaultAsync();
            if (appSettings == null) return NotFound();
            appSettings.CurrentTemplateId = id;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
