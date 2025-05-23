using System.ComponentModel.DataAnnotations;

namespace MediCode.Models
{
    public class Choroba
    {
        [Key]
        public int Id { get; set; }
        public int PacjentId { get; set; }
        public Pacjent? Pacjent { get; set; }
        [Display(Name = "Nazwa choroby")]
        public string Nazwa { get; set; }
        [Display(Name = "Opis choroby")]
        public string Opis { get; set; }
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
    }
}