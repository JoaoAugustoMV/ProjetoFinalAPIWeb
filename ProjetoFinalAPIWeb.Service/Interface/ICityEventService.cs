using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoFinalAPIWeb.Service.Entity;
using ProjetoFinalAPIWeb.Service.Enum;

namespace ProjetoFinalAPIWeb.Service.Interface
{
    public interface ICityEventService
    {
        #region Obter
        Task<IEnumerable<CityEventEntity>> ObterTodos();
        Task<CityEventEntity> ObterPorId(long idEvent);
        Task<IEnumerable<CityEventEntity>> ObterPorLocalData(string local, DateTime data);        
        Task<IEnumerable<CityEventEntity>> ObterPorTitulo(string termo);
        Task<IEnumerable<CityEventEntity>> ObterPorPrecoData(decimal precoMinimo, decimal precoMaximo, DateTime data);
        Task<bool> AdicionarEvento(CityEventEntity cityEvent);
        Task<bool> AtualizarEvento(long idEvent, CityEventEntity cityEvent);
        Task<RetornoDeleteEvent> ExcluirOuInativar(long idEvent);
        #endregion
        Task<bool> EventoExiste(long idEvent);
        Task<bool> EventoDisponivel(long idEvent);
        
    }
}
