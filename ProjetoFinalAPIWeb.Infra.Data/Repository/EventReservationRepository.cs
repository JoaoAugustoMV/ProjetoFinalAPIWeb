using ProjetoFinalAPIWeb.Service.Interface;
using ProjetoFinalAPIWeb.Service.Entity;
using Dapper;
using MySqlConnector;
using ProjetoFinalAPIWeb.Service.DTO;

namespace ProjetoFinalAPIWeb.Repository
{
    public class EventReservationRepository: IEventReservationRepository
    {
        private string stringConn = Environment.GetEnvironmentVariable("MySQL_Teste");

        public async Task<IEnumerable<ReservationWithEventTitle>> ObterPorNomeTitulo(string nome, string termo)
        {
            string query = "SELECT IdReservation, eventReservations.IdEvent, PersonName, Quantity, Title  FROM cityevents INNER JOIN eventReservations ON cityevents.IdEvent = eventreservations.IdEvent " +
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
            string query = "INSERT INTO EventReservations (IdEvent, PersonName, Quantity) VALUES (@idEvent, @PersonName, @Quantity)";

            DynamicParameters param = new(eventReservation);

            using MySqlConnection conn = new(stringConn);

            return await conn.ExecuteAsync(query, param) > 0;
            
        }        

        public async Task<bool> AtualizarQuantidadeReserva(long id, long quantidade)
        {
            string query = "UPDATE EventReservations SET Quantity = @quantidade WHERE idReservation = @id";
            DynamicParameters param = new();
            param.Add("id", id);
            param.Add("quantidade", quantidade);

            using MySqlConnection conn = new(stringConn);

            return await conn.ExecuteAsync(query, param) > 0;
        }

        public async Task<bool> RemoverReserva(long id)
        {
            string query = "DELETE FROM EventReservations WHERE idReservation = @id";

            DynamicParameters param = new();
            param.Add("id", id);

            using MySqlConnection conn = new(stringConn);

            
            return await conn.ExecuteAsync(query, param) > 0;
        }


    }
}
