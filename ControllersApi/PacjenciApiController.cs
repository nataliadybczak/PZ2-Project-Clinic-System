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
    public class PacjenciApiController : ControllerBase
    {
        private readonly MediCodeContext _context;

        public PacjenciApiController(MediCodeContext context)
        {
            _context = context;
        }

        // GET: api/PacjenciApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pacjent>>> GetPacjenci([FromHeader] string login, [FromHeader] string token)
        {

            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }
            var pacjenci = _context.Pacjenci
        .Include(p => p.DodanyPrzez)
        .Select(p => new
        {
            p.Id,
            p.Imie,
            p.Nazwisko,
            p.Pesel,
            DodanyPrzez = p.DodanyPrzez != null ? $"{p.DodanyPrzez.Imie} {p.DodanyPrzez.Nazwisko}" : "Nieznany"
        }).ToListAsync();
            // return await _context.Pacjenci.ToListAsync();
            return Ok(await pacjenci);
        }

        // GET: api/PacjenciApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pacjent>> GetPacjent(int id, [FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }

            if (_context.Pacjenci == null)
            {
                return NotFound();
            }

            // var pacjent = await _context.Pacjenci.FindAsync(id);
            // if (pacjent == null)
            // {
            //     return NotFound();
            // }
            // Jeśli chcesz zwrócić pacjenta z dodatkowymi informacjami, możesz użyć Include i Select
            var pacjent = await _context.Pacjenci
                .Include(p => p.DodanyPrzez)
                .Include(p => p.Choroby)
                .Include(p => p.Wizyty)
                .ThenInclude(w => w.Lekarz)
                .Select(p => new
                {
                    p.Id,
                    p.Imie,
                    p.Nazwisko,
                    p.Pesel,
                    DodanyPrzez = p.DodanyPrzez != null ? $"{p.DodanyPrzez.Imie} {p.DodanyPrzez.Nazwisko}" : "Nieznany",
                    Choroby = p.Choroby
                    .Select(c => new
                    {
                        c.Id,
                        c.Nazwa,
                        c.Opis,
                        DataRozpoznania = c.Data.HasValue ? c.Data.Value.ToString("yyyy-MM-dd") : null
                    }).ToList(),
                    Wizyty = p.Wizyty
                    .Select(w => new
                    {
                        w.Id,
                        DataWizyty = w.DataWizyty.ToString("yyyy-MM-dd HH:mm"),
                        w.Status,
                        Lekarz = w.Lekarz != null ? $"{w.Lekarz.Imie} {w.Lekarz.Nazwisko} - {w.Lekarz.Specjalizacja}" : "Nieznany"
                    }).ToList()
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pacjent == null)
            {
                return NotFound();
            }
            return Ok(pacjent);
        }

        // PUT: api/PacjenciApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPacjent(int id, [FromBody] Pacjent pacjent, [FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }

            if (_context.Pacjenci == null)
            {
                return NotFound();
            }
            var existingPacjent = await _context.Pacjenci.FindAsync(id);
            if (existingPacjent == null)
            {
                return NotFound();
            }
            existingPacjent.Imie = pacjent.Imie;
            existingPacjent.Nazwisko = pacjent.Nazwisko;
            existingPacjent.Pesel = pacjent.Pesel;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PacjentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new
            {
                existingPacjent.Id,
                existingPacjent.Imie,
                existingPacjent.Nazwisko,
                existingPacjent.Pesel
            });
        }

        // POST: api/PacjenciApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pacjent>> PostPacjent([FromBody] Pacjent pacjent, [FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }

            // Ustawienie lekarza dodającego pacjenta
            pacjent.DodanyPrzezId = lekarz.Id;
            // pacjent.DodanyPrzez = lekarz;
            _context.Pacjenci.Add(pacjent);
            await _context.SaveChangesAsync();
            // return CreatedAtAction("GetPacjent", new { id = pacjent.Id }, pacjent);
            return CreatedAtAction("GetPacjent", new { id = pacjent.Id }, new
            {
                pacjent.Id,
                pacjent.Imie,
                pacjent.Nazwisko,
                pacjent.Pesel
            });
        }

        // DELETE: api/PacjenciApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePacjent(int id, [FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }

            if (id <= 0)
            {
                return BadRequest("Nieprawidłowe ID pacjenta.");
            }
            if (_context.Pacjenci == null)
            {
                return NotFound();
            }
            var pacjent = await _context.Pacjenci.FindAsync(id);
            if (pacjent == null)
            {
                return NotFound();
            }

            _context.Pacjenci.Remove(pacjent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/PacjenciApi/{id}/choroby
        [HttpGet("{id}/choroby")]
        public async Task<ActionResult<IEnumerable<object>>> GetChoroby(int id, [FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }
            var pacjent = await _context.Pacjenci.FindAsync(id);
            if (pacjent == null)
            {
                return NotFound("Pacjent nie został znaleziony.");
            }
            var choroby = await _context.Choroby
                .Where(c => c.PacjentId == id)
                .Include(c => c.Pacjent)
                .Select(c => new
                {
                    c.Id,
                    Pacjent = c.Pacjent != null ? $"{c.Pacjent.Imie} {c.Pacjent.Nazwisko}" : "Nieznany",
                    c.Nazwa,
                    DataRozpoznania = c.Data.HasValue ? c.Data.Value.ToString("yyyy-MM-dd") : "Nieznana",
                    c.Opis
                })
                .ToListAsync();
            return Ok(choroby);
        }

        // POST: api/PacjenciApi/{id}/choroby
        [HttpPost("{id}/choroby")]
        public async Task<ActionResult<Choroba>> PostChoroba(int id, [FromBody] Choroba choroba, [FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }
            var pacjent = await _context.Pacjenci.FindAsync(id);
            if (pacjent == null)
            {
                return NotFound("Pacjent nie został znaleziony.");
            }
            choroba.PacjentId = id;
            _context.Choroby.Add(choroba);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetChoroby), new { id = pacjent.Id }, new
            {
                choroba.Id,
                choroba.Nazwa,
                DataRozpoznania = choroba.Data.HasValue ? choroba.Data.Value.ToString("yyyy-MM-dd") : "Nieznana",
                choroba.Opis
            });
        }


        private bool PacjentExists(int id)
        {
            return (_context.Pacjenci?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
