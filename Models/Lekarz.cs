using System.ComponentModel.DataAnnotations;

namespace MediCode.Models
{
    public class Lekarz
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Imię jest wymagane.")]
        public string Imie { get; set; }
        [Display(Name = "Administrator")]
        public bool IsAdmin { get; set; }
        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public string Nazwisko { get; set; }
        [Display(Name = "Specjalizacja")]
        [Required(ErrorMessage = "Specjalizacja jest wymagana.")]
        public string Specjalizacja { get; set; }
        [Required(ErrorMessage = "Login jest wymagany.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        public string Haslo { get; set; }
        public ICollection<Pacjent>? DodaniPacjenci { get; set; }
        public ICollection<Wizyta>? Wizyty { get; set; }
        [Display(Name = "Token")]
        public string? Token { get; set; }
    }
}