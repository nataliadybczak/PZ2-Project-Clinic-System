using System.ComponentModel.DataAnnotations;

public enum StatusWizyty
{
    [Display(Name = "Dostępna")]
    Dostepna,
    [Display(Name = "Zarezerwowana")]
    Zarezerwowana,
    [Display(Name = "Odwołana")]
    Odwolana,
    [Display(Name = "Zrealizowana")]
    Zrealizowana
}