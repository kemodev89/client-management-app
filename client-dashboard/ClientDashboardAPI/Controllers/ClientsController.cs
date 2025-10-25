using System.Linq;
using System.Threading.Tasks;
using ClientDashboardAPI.Data;
using ClientDashboardAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ClientsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /api/clients
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Clients
                .Include(c => c.PhoneNumbers)
                .ToListAsync();

            return Ok(list);
        }

        // GET: /api/clients/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var c = await _db.Clients
                .Include(x => x.PhoneNumbers)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (c == null)
                return NotFound();

            return Ok(c);
        }

        // POST: /api/clients
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Client dto)
        {
            _db.Clients.Add(dto);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOne), new { id = dto.Id }, dto);
        }

        // PUT: /api/clients/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client dto)
        {
            var c = await _db.Clients
                .Include(x => x.PhoneNumbers)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (c == null)
                return NotFound();

            // update scalar fields
            c.FirstName = dto.FirstName;
            c.LastName = dto.LastName;
            c.Email = dto.Email;

            // remove phone numbers that are missing in incoming payload
            var incomingIds = dto.PhoneNumbers.Select(p => p.Id).ToHashSet();
            var toRemove = c.PhoneNumbers.Where(p => !incomingIds.Contains(p.Id)).ToList();
            if (toRemove.Count > 0)
                _db.PhoneNumbers.RemoveRange(toRemove);

            // add or update numbers
            foreach (var p in dto.PhoneNumbers)
            {
                if (p.Id == 0)
                {
                    c.PhoneNumbers.Add(new PhoneNumber
                    {
                        Label = p.Label,
                        Number = p.Number
                    });
                }
                else
                {
                    var existing = c.PhoneNumbers.First(x => x.Id == p.Id);
                    existing.Label = p.Label;
                    existing.Number = p.Number;
                }
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: /api/clients/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Clients.FindAsync(id);
            if (c == null)
                return NotFound();

            _db.Clients.Remove(c); // cascade will delete phone numbers
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
