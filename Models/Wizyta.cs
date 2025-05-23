using System.ComponentModel.DataAnnotations;

namespace MediCode.Models
{
    public class Wizyta
    {
        [Key]
        public int Id { get; set; }
        public int? PacjentId { get; set; }
        public Pacjent? Pacjent { get; set; }
        public int LekarzId { get; set; }
        public Lekarz? Lekarz { get; set; }
        [Display(Name = "Data wizyty")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DataWizyty { get; set; }
        [Display(Name = "Status wizyty")]
        public StatusWizyty Status { get; set; }
    }
}