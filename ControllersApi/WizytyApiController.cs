using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCode.Data;
using MediCode.Models;

namespace MediCode.ControllersApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class WizytyApiController : ControllerBase
    {
        private readonly MediCodeContext _context;

        public WizytyApiController(MediCodeContext context)
        {
            _context = context;
        }

        // GET: api/WizytyApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetWizyty([FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }
            var wizyty = await _context.Wizyty
                .Include(w => w.Pacjent)
                .Where(w => w.LekarzId == lekarz.Id)
                .Select(w => new
                {
                    w.Id,
                    DataWizyty = w.DataWizyty.ToString("yyyy-MM-dd HH:mm"),
                    w.Status,
                    Pacjent = w.Pacjent != null ? new
                    {
                        w.Pacjent.Id,
                        w.Pacjent.Imie,
                        w.Pacjent.Nazwisko
                    } : null
                })
                .ToListAsync();
            return Ok(wizyty);
        }

        // GET: api/WizytyApi/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Wizyta>> GetWizyta(int id, [FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }

            // Sprawdzenie, czy wizyta należy do lekarza
            var wizyta = await _context.Wizyty
                .Include(w => w.Pacjent)
                .FirstOrDefaultAsync(w => w.Id == id && w.LekarzId == lekarz.Id);

            if (wizyta == null)
            {
                return NotFound("Nie znaleziono wizyty lub nie należy ona do tego lekarza.");
            }

            return Ok(new
            {
                wizyta.Id,
                DataWizyty = wizyta.DataWizyty.ToString("yyyy-MM-dd HH:mm"),
                wizyta.Status,
                Pacjent = wizyta.Pacjent != null ? new
                {
                    wizyta.Pacjent.Id,
                    wizyta.Pacjent.Imie,
                    wizyta.Pacjent.Nazwisko
                } : null
            });
        }

        // POST: api/WizytyApi
        [HttpPost]
        public async Task<ActionResult<Wizyta>> PostWizyta([FromBody] Wizyta wizyta, [FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }
            if (wizyta == null)
            {
                return BadRequest("Nieprawidłowe dane wizyty.");
            }

            wizyta.LekarzId = lekarz.Id;
            _context.Wizyty.Add(wizyta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWizyta), new
            {
                wizyta.Id,
                wizyta.DataWizyty,
                wizyta.Status,
                wizyta.PacjentId
            });
        }

        // PUT: api/WizytyApi/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWizyta(int id, [FromBody] Wizyta wizyta, [FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }
            if (id != wizyta.Id)
            {
                return BadRequest("Id wizyty nie zgadza się.");
            }

            var existingWizyta = await _context.Wizyty.FindAsync(id);
            if (existingWizyta == null || existingWizyta.LekarzId != lekarz.Id)
            {
                return NotFound("Nie znaleziono wizyty lub nie należy ona do tego lekarza.");
            }

            existingWizyta.DataWizyty = wizyta.DataWizyty;
            existingWizyta.Status = wizyta.Status;
            existingWizyta.PacjentId = wizyta.PacjentId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Wizyty.Any(w => w.Id == id && w.LekarzId == lekarz.Id))
                {
                    return NotFound("Nie znaleziono wizyty lub nie należy ona do tego lekarza.");
                }
                else
                {
                    throw;
                }
            }

            return Ok(new
            {
                existingWizyta.Id,
                DataWizyty = existingWizyta.DataWizyty.ToString("yyyy-MM-dd HH:mm"),
                existingWizyta.Status,
                existingWizyta.PacjentId
            });
        }

        // DELETE: api/WizytyApi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWizyta(int id, [FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }

            var wizyta = await _context.Wizyty.FindAsync(id);
            if (wizyta == null || wizyta.LekarzId != lekarz.Id)
            {
                return NotFound("Nie znaleziono wizyty lub nie należy ona do tego lekarza.");
            }

            _context.Wizyty.Remove(wizyta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}