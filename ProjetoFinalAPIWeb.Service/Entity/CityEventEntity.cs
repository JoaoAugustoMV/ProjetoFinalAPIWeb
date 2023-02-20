using System.ComponentModel.DataAnnotations;

namespace ProjetoFinalAPIWeb.Service.Entity
{
    public class CityEventEntity
    {
        public long IdEvent { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        [StringLength(150)]
        public string? Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateHourEvent { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string Local { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? Address { get; set; }                
        public decimal? Price { get; set; }
        public bool Status{ get; set; }

    }
        //public ICollection<EventReservation>? EventReservations { get; set; }
}
