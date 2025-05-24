using Microsoft.AspNetCore.Mvc;
using MediCode.Data;
using MediCode.Models;

namespace MediCode.ControllersApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class LekarzeApiController : ControllerBase
    {
        private readonly MediCodeContext _context;

        public LekarzeApiController(MediCodeContext context)
        {
            _context = context;
        }

        // GET: api/LekarzeApi
        [HttpGet]
        public IActionResult GetUserData([FromHeader] string login, [FromHeader] string token)
        {
            var lekarz = _context.Lekarze
                .FirstOrDefault(l => l.Login == login && l.Token == token);
            if (lekarz == null)
            {
                return Unauthorized("Niepoprawny login lub token.");
            }
            return Ok(new
            {
                lekarz.Id,
                lekarz.Login,
                lekarz.Imie,
                lekarz.Nazwisko,
                lekarz.Specjalizacja,
                lekarz.IsAdmin
            });
        }
    }
}