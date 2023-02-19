using ProjetoFinalAPIWeb.Service.DTO;
using ProjetoFinalAPIWeb.Service.Entity;

namespace ProjetoFinalAPIWeb.Service.Interface
{
    public interface IEventReservationRepository
    {                
        Task<IEnumerable<ReservationWithEventTitle>> ObterPorNomeTitulo(string nome, string termo);
        Task<bool> AdicionarReserva(EventReservationEntity eventReservation);        
        Task<bool> AtualizarQuantidadeReserva(long id, long quantidade);                
        Task<bool> RemoverReserva(long id);        
    }
}
