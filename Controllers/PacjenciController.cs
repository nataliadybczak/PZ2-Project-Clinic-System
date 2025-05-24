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
    public class PacjenciController : Controller
    {
        private readonly MediCodeContext _context;

        public PacjenciController(MediCodeContext context)
        {
            _context = context;
        }

        // GET: Pacjenci
        public async Task<IActionResult> Index()
        {
            var mediCodeContext = _context.Pacjenci.Include(p => p.DodanyPrzez);
            return View(await mediCodeContext.ToListAsync());
        }

        // GET: Pacjenci/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pacjenci == null)
            {
                return NotFound();
            }
            var lekarzId = HttpContext.Session.GetInt32("UserId");
            if (lekarzId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var pacjent = await _context.Pacjenci
                .Include(p => p.DodanyPrzez)
                .Include(p => p.Choroby)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pacjent == null)
            {
                return NotFound();
            }
            ViewBag.WizytyPacjenta = await _context.Wizyty
                .Where(w => w.PacjentId == id && w.LekarzId == lekarzId)
                .ToListAsync();

            return View(pacjent);
        }

        // GET: Pacjenci/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pacjenci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,Nazwisko,Pesel")] Pacjent pacjent)
        {
            var lekarzId = HttpContext.Session.GetInt32("UserId");
            if (lekarzId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            pacjent.DodanyPrzezId = (int)lekarzId;
            if (ModelState.IsValid)
            {
                _context.Add(pacjent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pacjent);
        }

        // GET: Pacjenci/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pacjenci == null)
            {
                return NotFound();
            }

            var pacjent = await _context.Pacjenci.Include(p => p.Choroby).FirstOrDefaultAsync(p => p.Id == id);
            if (pacjent == null)
            {
                return NotFound();
            }
            ViewBag.Choroby = await _context.Choroby.Select(c => c.Nazwa).Distinct().ToListAsync();
            return View(pacjent);
        }

        // POST: Pacjenci/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Imie,Nazwisko,Pesel")] Pacjent pacjent, string? wybranaChoroba, string? nowaChorobaNazwa, string? nowaChorobaOpis, DateTime? nowaChorobaData)
        {
            if (id != pacjent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pacjentToUpdate = await _context.Pacjenci.Include(p => p.Choroby).FirstOrDefaultAsync(p => p.Id == id);
                    if (pacjentToUpdate == null)
                    {
                        return NotFound();
                    }
                    pacjentToUpdate.Imie = pacjent.Imie;
                    pacjentToUpdate.Nazwisko = pacjent.Nazwisko;
                    pacjentToUpdate.Pesel = pacjent.Pesel;

                    var nazwaChoroby = !string.IsNullOrWhiteSpace(nowaChorobaNazwa) ? nowaChorobaNazwa : wybranaChoroba;
                    if (!string.IsNullOrWhiteSpace(nazwaChoroby))
                    {
                        pacjentToUpdate.Choroby.Add(new Choroba
                        {
                            Nazwa = nazwaChoroby,
                            Opis = nowaChorobaOpis,
                            Data = nowaChorobaData,
                            PacjentId = id
                        });
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacjentExists(pacjent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(pacjent);
        }

        // GET: Pacjenci/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pacjenci == null)
            {
                return NotFound();
            }

            var pacjent = await _context.Pacjenci
                .Include(p => p.DodanyPrzez)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pacjent == null)
            {
                return NotFound();
            }

            return View(pacjent);
        }

        // POST: Pacjenci/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pacjenci == null)
            {
                return Problem("Entity set 'MediCodeContext.Pacjenci'  is null.");
            }
            var pacjent = await _context.Pacjenci.FindAsync(id);
            if (pacjent != null)
            {
                _context.Pacjenci.Remove(pacjent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacjentExists(int id)
        {
            return (_context.Pacjenci?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
