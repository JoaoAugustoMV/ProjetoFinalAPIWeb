using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoFinalAPIWeb.Service.Entity;

namespace ProjetoFinalAPIWeb.Service.DTO
{
    public class ReservationWithEventTitle: EventReservationEntity
    {
        public string Title { get; set; }
    }
}
