using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoFinalAPIWeb.Service.Entity
{
    public class EventReservationEntity
    {
        public long IdReservation { get; set; }
        public long IdEvent{ get; set; }
        public string PersonName { get; set; }
        public long Quantity { get; set; }
    }
}
