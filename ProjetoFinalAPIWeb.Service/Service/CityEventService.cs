using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoFinalAPIWeb.Service.Entity;
using ProjetoFinalAPIWeb.Service.Enum;
using ProjetoFinalAPIWeb.Service.Interface;

namespace ProjetoFinalAPIWeb.Service.Service
{
    public class CityEventService: ICityEventService
    {
        private readonly ICityEventRepository _repository;

        public CityEventService(ICityEventRepository repository)
        {
            _repository = repository;
        }

        #region Obter
        public async Task<IEnumerable<CityEventEntity>> ObterTodos()
        {
            return await _repository.ObterTodos();
        }

        public async Task<CityEventEntity> ObterPorId(long idEvent)
        {
            return await _repository.ObterPorId(idEvent);
        }

        public async Task<IEnumerable<CityEventEntity>> ObterPorLocalData(string local, DateTime data)
        {
            return await _repository.ObterPorLocalData(local, data);
        }      
        public async Task<IEnumerable<CityEventEntity>> ObterPorPrecoData(decimal precoMaximo, decimal precoMinimo, DateTime data)
        {
            return await _repository.ObterPorPrecoData(precoMaximo, precoMinimo, data);
        }

        public async Task<IEnumerable<CityEventEntity>> ObterPorTitulo(string termo)
        {            
            return await _repository.ObterPorTitulo(termo);
        }
        #endregion
        public async Task<bool> AdicionarEvento(CityEventEntity cityEvent)
        {
            return await _repository.AdicionarEvento(cityEvent);
        }

        public async Task<bool> AtualizarEvento(long idEvent, CityEventEntity cityEvent)
        {
            return await _repository.AtualizarEvento(idEvent, cityEvent);
        }

        public async Task<RetornoDeleteEvent> ExcluirOuInativar(long idEvent)
        {
            if(!await EventoExiste(idEvent))
            {
                return RetornoDeleteEvent.NaoEncontrado;
            }
            
            if(await _repository.PossuiReservas(idEvent))
            {                
                return RetornoDeleteEvent.Inativado;
            }
            await _repository.ExcluirEvento(idEvent);
            return RetornoDeleteEvent.Removido;
        }          

        public bool ValidarData(DateTime date)
        {
            return date > DateTime.Now;
        }

        public bool ValidarPreco(decimal preco)
        {
            return preco >= 0;
        }

        public async Task<bool> EventoExiste(long idEvent)
        {
            return await _repository.ObterPorId(idEvent) != null;
        }

        public async Task<bool> EventoDisponivel(long idEvent)
        {
            CityEventEntity evento = await _repository.ObterPorId(idEvent);
            return evento.Status;
        }
    }
}
