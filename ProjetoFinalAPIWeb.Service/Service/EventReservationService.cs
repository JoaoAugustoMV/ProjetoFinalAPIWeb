using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoFinalAPIWeb.Service.DTO;
using ProjetoFinalAPIWeb.Service.Entity;
using ProjetoFinalAPIWeb.Service.Interface;

namespace ProjetoFinalAPIWeb.Service.Service
{
    public class EventReservationService: IEventReservationService
    {
        private readonly ICityEventRepository _repositoryEvent;
        private readonly IEventReservationRepository _repositoryReservation;

        public EventReservationService(ICityEventRepository repositoryEvent, IEventReservationRepository eventReservationRepository)
        {
            _repositoryEvent = repositoryEvent;
            _repositoryReservation = eventReservationRepository;
        }

        public async Task<bool> AdicionarReserva(EventReservationEntity eventReservation)
        {
            return await _repositoryReservation.AdicionarReserva(eventReservation);
        }

        public async Task<bool> AtualizarQuantidadeReserva(long id, long quantidade)
        {
            return await _repositoryReservation.AtualizarQuantidadeReserva(id, quantidade);
        }

        public async Task<bool> EventoDisponivel(long idEvent)
        {
            CityEventEntity evento = await _repositoryEvent.ObterPorId(idEvent);
            return evento.Status;
        }

        public async Task<bool> EventoExiste(long idEvent)
        {
            return await _repositoryEvent.ObterPorId(idEvent) != null;            
        }

        public async Task<IEnumerable<ReservationWithEventTitle>> ObterPorNomeTitulo(string nome, string termo)
        {
            return await _repositoryReservation.ObterPorNomeTitulo(nome, termo);
        }

        public async Task<bool> RemoverReserva(long id)
        {
            return await _repositoryReservation.RemoverReserva(id);
        }

        public bool ValidarQuantidade(long quantidade)
        {
            return quantidade > 0;
        }
    }
}
