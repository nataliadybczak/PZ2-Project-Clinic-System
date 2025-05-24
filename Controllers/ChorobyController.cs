using Microsoft.AspNetCore.Mvc;
using MediCode.Data;
using MediCode.Models;
namespace MediCode.Controllers
{
    public class ChorobyController : Controller
    {
        private readonly MediCodeContext _context;

        public ChorobyController(MediCodeContext context)
        {
            _context = context;
        }

        // GET: Choroby/EditOpis/5
        public async Task<IActionResult> EditOpis(int? id)
        {
            if (id == null) return NotFound();

            var choroba = await _context.Choroby.FindAsync(id);
            if (choroba == null) return NotFound();

            return View(choroba);
        }

        // POST: Choroby/EditOpis/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOpis(int id, [Bind("Id,Opis")] Choroba choroba)
        {
            var chorobaToUpdate = await _context.Choroby.FindAsync(id);
            if (chorobaToUpdate == null) return NotFound();

            chorobaToUpdate.Opis = choroba.Opis;
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Pacjenci", new { id = chorobaToUpdate.PacjentId });
        }
    }
}