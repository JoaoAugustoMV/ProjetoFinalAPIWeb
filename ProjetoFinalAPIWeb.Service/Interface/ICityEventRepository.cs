using ProjetoFinalAPIWeb.Service.Entity;
using ProjetoFinalAPIWeb.Service.Enum;

namespace ProjetoFinalAPIWeb.Service.Interface
{
    public interface ICityEventRepository
    {
        #region Obter
        Task<IEnumerable<CityEventEntity>> ObterTodos();
        Task<CityEventEntity> ObterPorId(long idEvent);        
        Task<IEnumerable<CityEventEntity>> ObterPorLocalData(string local, DateTime data);                     
        Task<IEnumerable<CityEventEntity>> ObterPorTitulo(string termo);        
        Task<IEnumerable<CityEventEntity>> ObterPorPrecoData(decimal precoMinimo, decimal precoMaximo, DateTime data);        
        #endregion
        Task<bool> AdicionarEvento(CityEventEntity cityEvent);        
        Task<bool> AtualizarEvento(long idEvent, CityEventEntity cityEvent);        
        Task<bool> ExcluirEvento(long idEvent);
        Task<bool> InativarEvento(long idEvent);
        Task<bool> PossuiReservas(long idEvent);         
        
        #region Filtros
        Task<IEnumerable<CityEventEntity>> FiltrarPorData(DateTime data, IEnumerable<CityEventEntity>? eventos = null);               
        Task<IEnumerable<CityEventEntity>> FiltrarPorPreco(decimal precoMinimo, decimal precoMaximo, IEnumerable<CityEventEntity> eventos = null);      
        Task<IEnumerable<CityEventEntity>> FiltrarPorTitulo(string termo, IEnumerable<CityEventEntity> eventos = null);      
        Task<IEnumerable<CityEventEntity>> FiltrarPorLocal(string local, IEnumerable<CityEventEntity> eventos = null);
        #endregion
        
    }
}
