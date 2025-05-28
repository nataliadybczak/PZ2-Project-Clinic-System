using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediCode.Data;
using MediCode.Models;
using System.Security.Cryptography;
using System.Text;

namespace MediCode.Controllers
{
    public class LekarzeController : Controller
    {
        private readonly MediCodeContext _context;

        public LekarzeController(MediCodeContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "True";
        }

        // GET: Lekarze
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return _context.Lekarze != null ?
                        View(await _context.Lekarze.ToListAsync()) :
                        Problem("Entity set 'MediCodeContext.Lekarze'  is null.");
        }

        // GET: Lekarze/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (id == null || _context.Lekarze == null)
            {
                return NotFound();
            }

            var lekarz = await _context.Lekarze
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lekarz == null)
            {
                return NotFound();
            }
            return View(lekarz);
        }

        // GET: Lekarze/Create
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return View();
        }

        // POST: Lekarze/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,IsAdmin,Nazwisko,Specjalizacja,Login,Haslo")] Lekarz lekarz)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if (ModelState.IsValid)
            {
                lekarz.Haslo = HashPassword(lekarz.Haslo);
                lekarz.IsAdmin = false; // Domyślnie ustawiamy IsAdmin na false
                lekarz.Token = Guid.NewGuid().ToString(); // Generowanie unikalnego tokena
                _context.Add(lekarz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lekarz);
        }

        // GET: Lekarze/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (id == null || _context.Lekarze == null)
            {
                return NotFound();
            }

            var lekarz = await _context.Lekarze.FindAsync(id);
            if (lekarz == null)
            {
                return NotFound();
            }
            return View(lekarz);
        }

        // POST: Lekarze/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("AccessDenied", "Home");

            var lekarzToUpdate = await _context.Lekarze.FindAsync(id);
            if (lekarzToUpdate == null)
                return NotFound();

            // Ręczne przypisanie z formularza
            if (await TryUpdateModelAsync(lekarzToUpdate, "",
                l => l.Imie, l => l.Nazwisko, l => l.Specjalizacja))
            {
                // Login nieedytowalny – pomijamy
                var noweHaslo = Request.Form["haslo"];
                if (!string.IsNullOrEmpty(noweHaslo))
                {
                    lekarzToUpdate.Haslo = HashPassword(noweHaslo);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // return View(lekarzToUpdate);
            return View(lekarzToUpdate);
        }

        // GET: Lekarze/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (id == null || _context.Lekarze == null)
            {
                return NotFound();
            }

            var lekarz = await _context.Lekarze
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lekarz == null)
            {
                return NotFound();
            }

            return View(lekarz);
        }

        // POST: Lekarze/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (_context.Lekarze == null)
            {
                return Problem("Entity set 'MediCodeContext.Lekarze'  is null.");
            }
            var lekarz = await _context.Lekarze.FindAsync(id);
            if (lekarz != null)
            {
                _context.Lekarze.Remove(lekarz);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LekarzExists(int id)
        {
            return (_context.Lekarze?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
