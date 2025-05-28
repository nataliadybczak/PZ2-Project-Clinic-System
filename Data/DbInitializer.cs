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

            // Hashowanie hasła
            string HashPassword(string password)
            {
                using var sha256 = SHA256.Create();
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }

            // Sprawdź, czy są już dane w bazie
            if (!context.Lekarze.Any())
            {
                // Dodaj admina
                var admin = new Lekarz
                {
                    Imie = "Adam",
                    Nazwisko = "Adminowski",
                    Specjalizacja = "Administrator",
                    Login = "admin",
                    Haslo = HashPassword("bardzotrudnehaslo"),
                    IsAdmin = true,
                    Token = Guid.NewGuid().ToString()
                };

                context.Lekarze.Add(admin);
                context.SaveChanges();
            }
            else
            {
                // Jeśli admin już istnieje, zaktualizuj jego token
                var admin = context.Lekarze.FirstOrDefault(l => l.Login == "admin");
                if (admin != null && string.IsNullOrEmpty(admin.Token))
                {
                    admin.Token = Guid.NewGuid().ToString();
                    context.SaveChanges();
                }
            }

            // Uzupełnij tokeny dla lekarzy, którzy jeszcze ich nie mają
            // var lekarzeBezTokena = context.Lekarze.Where(l => string.IsNullOrEmpty(l.Token)).ToList();
            // foreach (var lekarz in lekarzeBezTokena)
            // {
            //     lekarz.Token = Guid.NewGuid().ToString();
            // }
            // context.SaveChanges();

        }
    }
}