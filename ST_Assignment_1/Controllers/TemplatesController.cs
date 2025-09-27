using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST_Assignment_1.Data;
using ST_Assignment_1.Models;

namespace ST_Assignment_1.Controllers
{
    [ApiController]
    [Route("api/templates")]
    public class TemplatesController : ControllerBase
    {
        private readonly WorkoutJournalDbContext _db;
        public TemplatesController(WorkoutJournalDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Template>>> GetAll()
        {
            return await _db.Templates.Include(t => t.Items).ThenInclude(i => i.Exercise).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Template>> GetById(int id)
        {
            var template = await _db.Templates.Include(t => t.Items).ThenInclude(i => i.Exercise).FirstOrDefaultAsync(t => t.Id == id);
            if (template == null) return NotFound();
            return template;
        }

        [HttpPost]
        public async Task<ActionResult<Template>> Create(Template template)
        {
            _db.Templates.Add(template);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = template.Id }, template);
        }

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var template = await _db.Templates.FindAsync(id);
            if (template == null) return NotFound();
            _db.Templates.Remove(template);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // GET /api/templates/current
        [HttpGet("current")]
        public async Task<ActionResult<Template>> GetCurrent()
        {
            var appSettings = await _db.AppSettings.FirstOrDefaultAsync();
            if (appSettings == null) return NotFound();
            var template = await _db.Templates.Include(t => t.Items).ThenInclude(i => i.Exercise).FirstOrDefaultAsync(t => t.Id == appSettings.CurrentTemplateId);
            if (template == null) return NotFound();
            return template;
        }

        // PUT /api/templates/current/{id}
        [HttpPut("current/{id}")]
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
