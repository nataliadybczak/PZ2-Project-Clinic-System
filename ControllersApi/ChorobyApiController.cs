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
    public class ChorobyApiController : ControllerBase
    {
        private readonly MediCodeContext _context;

        public ChorobyApiController(MediCodeContext context)
        {
            _context = context;
        }

        // GET: api/ChorobyApi/{id}
        [HttpGet("choroby/{id}")]
        public async Task<IActionResult> GetChoroba(int id, [FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }
            var choroba = await _context.Choroby.FindAsync(id);
            if (choroba == null)
            {
                return NotFound("Choroba nie została znaleziona.");
            }
            return Ok(new
            {
                choroba.Id,
                choroba.Nazwa,
                DataRozpoznania = choroba.Data.HasValue ? choroba.Data.Value.ToString("yyyy-MM-dd") : "Nieznana",
                choroba.Opis
            });
        }

        // PUT: api/ChorobyApi/{id}
        [HttpPut("choroby/{id}")]
        public async Task<IActionResult> PutChoroba(int id, [FromBody] Choroba choroba, [FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }
            if (id != choroba.Id)
            {
                return BadRequest("Niepoprawne ID choroby.");
            }
            var existingChoroba = await _context.Choroby.FindAsync(id);
            if (existingChoroba == null)
            {
                return NotFound("Choroba nie została znaleziona.");
            }
            existingChoroba.Opis = choroba.Opis;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Wystąpił błąd podczas zapisu zmian.");
            }
            return Ok(new
            {
                existingChoroba.Id,
                existingChoroba.Nazwa,
                DataRozpoznania = existingChoroba.Data.HasValue ? existingChoroba.Data.Value.ToString("yyyy-MM-dd") : "Nieznana",
                existingChoroba.Opis
            });
        }
    }
}