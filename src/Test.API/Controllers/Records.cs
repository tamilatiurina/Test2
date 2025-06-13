using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.API.DAL;
using Test.API.DTOs;
using Test.API.Models;

namespace Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Records : ControllerBase
    {
        private readonly MyContext _context;

        public Records(MyContext context)
        {
            _context = context;
        }

        // GET: api/Records
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Record>>> GetRecord(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int? languageId,
            [FromQuery] int? taskId)
        {
            var query = _context.Record
                .Include(s => s.Language)
                .Include(s => s.Task)
                .Include(s => s.Student)
                .AsQueryable();

            if (from.HasValue)
                query = query.Where(s => s.CreatedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(s => s.CreatedAt <= to.Value);

            if (languageId.HasValue)
                query = query.Where(s => s.Language.Id == languageId.Value);

            if (taskId.HasValue)
                query = query.Where(s => s.Task.Id == taskId.Value);
            
            var result = await query
                .OrderByDescending(s => s.CreatedAt)
                .ThenBy(s => s.Student.LastName)
                .Select(s => new RecordDto()
                {
                    Id = s.Id,
                    Language = new LanguageDto { Id = s.Language.Id, Name = s.Language.Name },
                    Task = new TaskDto { Id = s.Task.Id, Name = s.Task.Name, Description = s.Task.Description },
                    Student = new StudentDto
                    {
                        Id = s.Student.Id,
                        FirstName = s.Student.FirstName,
                        LastName = s.Student.LastName,
                        Email = s.Student.Email
                    },
                    ExecutionTime = s.ExecutionTime,
                    Created = s.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss")
                })
                .ToListAsync();
            return Ok(result);
        }

        // GET: api/Records/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Record>> GetRecord(int id)
        {
            var @record = await _context.Record.FindAsync(id);

            if (@record == null)
            {
                return NotFound();
            }

            return @record;
        }

        // PUT: api/Records/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecord(int id, Record @record)
        {
            if (id != @record.Id)
            {
                return BadRequest();
            }

            _context.Entry(@record).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Records
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Record>> PostRecord(Record @record)
        {
            _context.Record.Add(@record);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecord", new { id = @record.Id }, @record);
        }

        // DELETE: api/Records/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            var @record = await _context.Record.FindAsync(id);
            if (@record == null)
            {
                return NotFound();
            }

            _context.Record.Remove(@record);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecordExists(int id)
        {
            return _context.Record.Any(e => e.Id == id);
        }
    }
}
