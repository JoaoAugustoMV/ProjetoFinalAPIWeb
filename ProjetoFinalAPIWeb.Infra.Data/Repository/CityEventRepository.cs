using Dapper;
using MySqlConnector;
using ProjetoFinalAPIWeb.Service.Interface;
using ProjetoFinalAPIWeb.Service.Entity;

namespace ProjetoFinalAPIWeb.Infra.Data.Repository
{
    public class CityEventRepository: ICityEventRepository
    {
        private string stringConn = Environment.GetEnvironmentVariable("DATABASE_CONFIG");

        #region Query
        public async Task<IEnumerable<CityEventEntity>> ObterTodos()
        {
            string query = "SELECT * FROM CityEvent";

            using MySqlConnection conn = new(stringConn);

            return await conn.QueryAsync<CityEventEntity>(query);
        }
        public async Task<CityEventEntity> ObterPorId(long idEvent)
        {
            string query = "SELECT * FROM CityEvent WHERE IdEvent = @idEvent";

            DynamicParameters param = new ();
            param.Add("idEvent", idEvent);

            using MySqlConnection conn = new(stringConn);
            return await conn.QueryFirstOrDefaultAsync<CityEventEntity>(query, param);
        }
        public async Task<IEnumerable<CityEventEntity>> ObterPorLocalData(string local, DateTime data)
        {
            
            IEnumerable<CityEventEntity> result = await FiltrarPorLocal(local);
            
            result = await FiltrarPorData(data, result);
         
            return result;
        }
        
        public async Task<IEnumerable<CityEventEntity>> ObterPorTitulo(string termo)
        {            
            return await FiltrarPorTitulo(termo);
        }

        public async Task<IEnumerable<CityEventEntity>> ObterPorPrecoData(decimal precoMaximo, decimal precoMinimo, DateTime data)
        {
            if(data == DateTime.MinValue) // Se não passou nenhuma data
            {
                return null;
            }
           
            IEnumerable<CityEventEntity> result = await FiltrarPorPreco(precoMaximo, precoMinimo);
            result = await FiltrarPorData(data, result); // Buscar por data exata

            return result;

        }
        public async Task<bool> AdicionarEvento(CityEventEntity cityEvent)
        {
            //if(cityEvent.DateHourEvent)
            string query = "INSERT INTO CityEvent " +
                "(Title, Description, DateHourEvent, Local, Address, Price, Status) VALUES " +
                "( @Title, @Description, @DateHourEvent, @Local, @Address,  @Price, @Status)";

            DynamicParameters param = new (cityEvent);

            using MySqlConnection conn = new(stringConn);
            
            int linhasAfetadas = await conn.ExecuteAsync(query, param);

            return linhasAfetadas > 0;
            
        }

        public async Task<bool> AtualizarEvento(long idEvent, CityEventEntity cityEvent)
        {
            string query = "UPDATE CityEvent SET Title = @Title, Description = @Description, DateHourEvent = @DateHourEvent, Local = @Local , Address = @Address, Price = @Price, Status = @Status WHERE IdEvent = @IdEvent";

            DynamicParameters param = new(cityEvent);
            param.Add("idEvent", idEvent);

            using MySqlConnection conn = new(stringConn);            

            return await conn.ExecuteAsync(query, param) > 0;
        }
        public async Task<bool> ExcluirEvento(long idEvent)
        {            
            string query = "DELETE FROM CityEvent WHERE IdEvent = @idEvent";

            DynamicParameters param = new();
            param.Add("idEvent", idEvent);
            using MySqlConnection conn = new(stringConn);

            return await conn.ExecuteAsync(query, param) > 0;            
        }
        
        public async Task<bool> InativarEvento(long idEvent)
        {
            string query = "UPDATE CityEvent SET Status = false WHERE idEvent = @idEvent";
            DynamicParameters param = new();
            param.Add("idEvent", idEvent);

            using MySqlConnection conn = new(stringConn);
            return await conn.ExecuteAsync(query, param) > 0;
        }
        #endregion
        public async Task<bool> PossuiReservas(long idEvent)      
        {
            string query = "SELECT * FROM EventReservation WHERE IdEvent = @idEvent";

            DynamicParameters param = new ();
            param.Add("idEvent", idEvent);

            using MySqlConnection conn = new(stringConn);   

            return await conn.QueryFirstOrDefaultAsync<EventReservationEntity>(query, param) != null;
        }


        #region Filtros

        // Os metodos de filtros foram feito de forma separada para poder encadea-los conforme a necessidade

        // Cada metodo retorna uma coleção filtrada, que esta pode ser passada para o proximo metodo de filtro
        public async Task<IEnumerable<CityEventEntity>> FiltrarPorPreco(decimal precoMaximo, decimal precoMinimo = 0, IEnumerable<CityEventEntity> eventos = null)
        {
            if(eventos == null)
            {
                string query = "SELECT * FROM CityEvent WHERE Price BETWEEN @precoMinimo AND @precoMaximo";

                DynamicParameters param = new();
                param.Add("precoMinimo", precoMinimo);
                param.Add("precoMaximo", precoMaximo);

                using MySqlConnection conn = new(stringConn);

                return await conn.QueryAsync<CityEventEntity>(query, param);                                             
            }

            return eventos.Where(e => precoMinimo <= e.Price && e.Price <= precoMaximo);
        }

        public async Task<IEnumerable<CityEventEntity>> FiltrarPorTitulo(string termo, IEnumerable<CityEventEntity> eventos = null)
        {
            
            if(eventos == null)
            {                
                string query = "SELECT * FROM CityEvent WHERE Title LIKE @termo";
                DynamicParameters param = new();
                param.Add("termo", "%" + termo + "%");

                using MySqlConnection conn = new(stringConn);

                return await conn.QueryAsync<CityEventEntity>(query, param);
            }
            return eventos.Where(e => e.Title.Contains(termo));
        }

        public async Task<IEnumerable<CityEventEntity>> FiltrarPorLocal(string local, IEnumerable<CityEventEntity> eventos = null)
        {
            string query = $"SELECT * FROM CityEvent WHERE Local LIKE @local";
                          //SELECT * FROM CityEvents WHERE Local LIKE '%tri%';
            if(eventos == null)
            {
                DynamicParameters param = new(local);
                param.Add("local", "%" + local + "%");
                
                using MySqlConnection conn = new(stringConn);

                
                return await conn.QueryAsync<CityEventEntity>(query, param);
                
            }

            return eventos.Where(e => e.Local.Contains(local));
        }

        public async Task<IEnumerable<CityEventEntity>> FiltrarPorData(DateTime data, IEnumerable<CityEventEntity>? eventos = null)
        {
            if(eventos == null)
            {
                string query = "SELECT * FROM CityEvent WHERE DateHourEvent = @data";
                DynamicParameters param = new(data);
                param.Add("data", data);
                using MySqlConnection conn = new(stringConn);

                return await conn.QueryAsync<CityEventEntity>(query, param);
            }
            
            return eventos.Where(e => e.DateHourEvent.Date == data);
        }



        #endregion
    }
}
