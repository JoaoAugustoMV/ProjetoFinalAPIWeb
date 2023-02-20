using ProjetoFinalAPIWeb.Service.Interface;
using ProjetoFinalAPIWeb.Service.Entity;
using Dapper;
using MySqlConnector;
using ProjetoFinalAPIWeb.Service.DTO;

namespace ProjetoFinalAPIWeb.Repository
{
    public class EventReservationRepository: IEventReservationRepository
    {
        private string stringConn = Environment.GetEnvironmentVariable("DATABASE_CONFIG");

        public async Task<IEnumerable<ReservationWithEventTitle>> ObterPorNomeTitulo(string nome, string termo)
        {
            string query = "SELECT IdReservation, EventReservation.IdEvent, PersonName, Quantity, Title  FROM CityEvent INNER JOIN EventReservation ON CityEvent.IdEvent = EventReservation.IdEvent " +
                "WHERE PersonName = @nome AND Title LIKE @termo";
            DynamicParameters param = new();

            param.Add("nome", nome);
            param.Add("termo", "%" + termo + "%");

            using MySqlConnection conn = new(stringConn);

            var result = conn.QueryAsync<ReservationWithEventTitle>(query, param);

            return await result;
        }
        public async Task<bool> AdicionarReserva(EventReservationEntity eventReservation)
        {
            string query = "INSERT INTO EventReservation (IdEvent, PersonName, Quantity) VALUES (@idEvent, @PersonName, @Quantity)";

            DynamicParameters param = new(eventReservation);

            using MySqlConnection conn = new(stringConn);

            return await conn.ExecuteAsync(query, param) > 0;
            
        }        

        public async Task<bool> AtualizarQuantidadeReserva(long id, long quantidade)
        {
            string query = "UPDATE EventReservation SET Quantity = @quantidade WHERE idReservation = @id";
            DynamicParameters param = new();
            param.Add("id", id);
            param.Add("quantidade", quantidade);

            using MySqlConnection conn = new(stringConn);
            Console.WriteLine(id + quantidade);
            return await conn.ExecuteAsync(query, param) > 0;
        }

        public async Task<bool> RemoverReserva(long id)
        {
            string query = "DELETE FROM EventReservation WHERE idReservation = @id";

            DynamicParameters param = new();
            param.Add("id", id);

            using MySqlConnection conn = new(stringConn);

            
            return await conn.ExecuteAsync(query, param) > 0;
        }


    }
}
