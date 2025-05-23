using MediCode.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MediCode.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MediCodeContext context)
        {
            context.Database.Migrate();

            // Sprawdź, czy są już dane w bazie
            if (context.Lekarze.Any())
            {
                return; // Baza danych już zawiera dane
            }

            // Hashowanie hasła
            string HashPassword(string password)
            {
                using var sha256 = SHA256.Create();
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }

            // Dodaj admina
            var admin = new Lekarz
            {
                Imie = "Adam",
                Nazwisko = "Adminowski",
                Specjalizacja = "Administrator",
                Login = "admin",
                Haslo = HashPassword("bardzotrudnehaslo"),
                IsAdmin = true
            };

            context.Lekarze.Add(admin);
            context.SaveChanges();
        }
    }
}