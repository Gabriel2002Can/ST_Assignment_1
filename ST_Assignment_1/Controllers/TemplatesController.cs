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
            return await _db.Templates.Include(t => t.Items).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Template>> GetById(Guid id)
        {
            var template = await _db.Templates.Include(t => t.Items).FirstOrDefaultAsync(t => t.Id == id);
            if (template == null) return NotFound();
            return template;
        }

        [HttpPost]
        public async Task<ActionResult<Template>> Create(Template template)
        {
            template.Id = Guid.NewGuid();
            if (template.Items != null)
            {
                foreach (var item in template.Items)
                {
                    item.Id = Guid.NewGuid();
                }
            }
            _db.Templates.Add(template);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = template.Id }, template);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Template updated)
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
                    item.Id = Guid.NewGuid();
                    template.Items.Add(item);
                }
            }
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var template = await _db.Templates.FindAsync(id);
            if (template == null) return NotFound();
            _db.Templates.Remove(template);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
