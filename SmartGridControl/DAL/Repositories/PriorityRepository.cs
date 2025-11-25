using DAL.Connection;
using DAL.Helpers;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories
{
    public class PriorityRepository : IPriorityRepository
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public PriorityRepository(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Priority>> GetAllAsync()
        {
            List<Priority> priorities = new List<Priority>();

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Priority", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                priorities.Add(DataReaderMappers.MapToPriority(reader)!);
            }

            return priorities;
        }

        public async Task<Priority?> GetByIdAsync(int id)
        {
            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Priority WHERE Id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            Priority priority = DataReaderMappers.MapToPriority(reader)!;

            return priority;
        }
    }
}
