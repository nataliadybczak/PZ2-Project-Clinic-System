using MediCode.Data;
using MediCode.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;


namespace MediCode.Controllers;

public class LoginController : Controller
{
    private readonly MediCodeContext _context;
    private readonly ILogger<LoginController> _logger;

    public LoginController(MediCodeContext context, ILogger<LoginController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(string login, string haslo)
    {
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(haslo))
        {
            ViewBag.ErrorMessage = "Login i hasło są wymagane.";
            return View();
        }

        // Sprawdzenie, czy użytkownik istnieje w bazie danych
        var hash = HashPassword(haslo);
        var lekarz = _context.Lekarze.FirstOrDefault(l => l.Login == login && l.Haslo == hash);

        if (lekarz != null)
        {
            // Ustawienie sesji dla lekarza
            HttpContext.Session.SetInt32("UserId", lekarz.Id);
            HttpContext.Session.SetInt32("LekarzId", lekarz.Id);
            HttpContext.Session.SetString("IsAdmin", lekarz.IsAdmin.ToString());
            _logger.LogInformation($"Użytkownik {lekarz.Imie} {lekarz.Nazwisko} zalogowany pomyślnie.");
            return RedirectToAction("Index", "Home");
        }

        // Jeśli użytkownik nie istnieje, wyświetl komunikat o błędzie
        _logger.LogWarning($"Nieudana próba logowania dla loginu: {login}");
        ViewBag.ErrorMessage = "Nieprawidłowy login lub hasło.";
        return View();
    }
    public IActionResult Logout()
    {
        // Usunięcie sesji
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    // Hashowanie hasła
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }
}