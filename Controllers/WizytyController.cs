using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediCode.Data;
using MediCode.Models;

namespace MediCode.Controllers
{
    public class WizytyController : Controller
    {
        private readonly MediCodeContext _context;

        public WizytyController(MediCodeContext context)
        {
            _context = context;
        }

        // GET: Wizyty
        public async Task<IActionResult> Index()
        {
            var lekarzId = HttpContext.Session.GetInt32("LekarzId");
            if (lekarzId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var wizyty = await _context.Wizyty
                .Include(w => w.Lekarz)
                .Include(w => w.Pacjent)
                .Where(w => w.LekarzId == lekarzId.Value)
                .ToListAsync();
            return View(wizyty);
        }

        // GET: Wizyty/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Wizyty == null)
            {
                return NotFound();
            }
            var lekarzId = HttpContext.Session.GetInt32("LekarzId");
            if (lekarzId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var wizyta = await _context.Wizyty
                .Include(w => w.Lekarz)
                .Include(w => w.Pacjent)
                .FirstOrDefaultAsync(m => m.Id == id && m.LekarzId == lekarzId.Value);
            if (wizyta == null)
            {
                return NotFound();
            }

            return View(wizyta);
        }

        // GET: Wizyty/Create
        public IActionResult Create()
        {
            ViewData["PacjentId"] = new SelectList(
                _context.Pacjenci.Select(p => new
                {
                    Id = p.Id,
                    Opis = p.Imie + " " + p.Nazwisko + " (" + p.Pesel + ")"
                }),
                "Id", "Opis"
            );
            return View();
        }

        // POST: Wizyty/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PacjentId,DataWizyty,Status")] Wizyta wizyta)
        {
            var lekarzId = HttpContext.Session.GetInt32("LekarzId");
            if (lekarzId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            wizyta.LekarzId = lekarzId.Value;
            if (ModelState.IsValid)
            {
                _context.Add(wizyta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // ViewData["PacjentId"] = new SelectList(_context.Pacjenci, "Id", "Imie", wizyta.PacjentId);
            ViewData["PacjentId"] = new SelectList(
                _context.Pacjenci.Select(p => new
                {
                    Id = p.Id,
                    Opis = p.Imie + " " + p.Nazwisko + " (" + p.Pesel + ")"
                }),
                "Id", "Opis"
            );
            return View(wizyta);
        }

        // GET: Wizyty/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Wizyty == null)
            {
                return NotFound();
            }
            var lekarzId = HttpContext.Session.GetInt32("LekarzId");
            if (lekarzId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var wizyta = await _context.Wizyty.FirstOrDefaultAsync(m => m.Id == id && m.LekarzId == lekarzId.Value);
            if (wizyta == null)
            {
                return NotFound();
            }
            ViewData["PacjentId"] = new SelectList(
                _context.Pacjenci.Select(p => new
                {
                    Id = p.Id,
                    Opis = p.Imie + " " + p.Nazwisko + " (" + p.Pesel + ")"
                }),
                "Id", "Opis"
            );
            return View(wizyta);
        }

        // POST: Wizyty/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PacjentId,DataWizyty,Status")] Wizyta wizyta)
        {
            if (id != wizyta.Id)
            {
                return NotFound();
            }
            var lekarzId = HttpContext.Session.GetInt32("LekarzId");
            if (lekarzId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var wizytaToUpdate = await _context.Wizyty.FirstOrDefaultAsync(m => m.Id == id && m.LekarzId == lekarzId.Value);
            if (wizytaToUpdate == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    wizytaToUpdate.PacjentId = wizyta.PacjentId;
                    wizytaToUpdate.DataWizyty = wizyta.DataWizyty;
                    wizytaToUpdate.Status = wizyta.Status;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WizytaExists(wizyta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacjentId"] = new SelectList(_context.Pacjenci, "Id", "Imie", wizyta.PacjentId);
            return View(wizyta);
        }

        // GET: Wizyty/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Wizyty == null)
            {
                return NotFound();
            }
            var lekarzId = HttpContext.Session.GetInt32("LekarzId");
            if (lekarzId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var wizyta = await _context.Wizyty
                .Include(w => w.Lekarz)
                .Include(w => w.Pacjent)
                .FirstOrDefaultAsync(m => m.Id == id && m.LekarzId == lekarzId.Value);
            if (wizyta == null)
            {
                return NotFound();
            }

            return View(wizyta);
        }

        // POST: Wizyty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Wizyty == null)
            {
                return Problem("Entity set 'MediCodeContext.Wizyty'  is null.");
            }
            var lekarzId = HttpContext.Session.GetInt32("LekarzId");
            if (lekarzId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var wizyta = await _context.Wizyty.FirstOrDefaultAsync(w => w.Id == id && w.LekarzId == lekarzId.Value);
            if (wizyta != null)
            {
                _context.Wizyty.Remove(wizyta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WizytaExists(int id)
        {
            return (_context.Wizyty?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
