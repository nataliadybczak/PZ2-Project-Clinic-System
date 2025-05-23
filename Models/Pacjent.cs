using System.ComponentModel.DataAnnotations;

namespace MediCode.Models
{
    public class Pacjent
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Imię jest wymagane.")]
        public string Imie { get; set; }
        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public string Nazwisko { get; set; }
        [Display(Name = "PESEL")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "PESEL musi mieć 11 cyfr.")]
        [Required(ErrorMessage = "PESEL jest wymagany.")]
        public string Pesel { get; set; }
        public int? DodanyPrzezId { get; set; }
        public Lekarz? DodanyPrzez { get; set; }
        public ICollection<Wizyta>? Wizyty { get; set; }
        public ICollection<Choroba>? Choroby { get; set; }
    }
}