using DAL.Connection;
using DAL.Helpers;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories
{
    public class StateRepository : IStateRepository
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public StateRepository(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<State>> GetAllAsync()
        {
            List<State> states = new List<State>();

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM State", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                states.Add(DataReaderMappers.MapToState(reader)!);
            }

            return states;
        }

        public async Task<State?> GetByIdAsync(int id)
        {
            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM State WHERE Id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            State state = DataReaderMappers.MapToState(reader)!;

            return state;
        }
    }
}
