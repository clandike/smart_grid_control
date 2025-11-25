using DAL.Connection;
using DAL.Helpers;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public ProjectRepository(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task DeleteAsync(int id)
        {
            var stringQuery = $"DELETE FROM Project WHERE Id = @id";
            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery, new { id });
        }

        public async Task CreateAsync(Project entity)
        {
            var stringQuery = @"
            INSERT INTO Project (Name, Location, TimeZone, UnitId, CreatedAt)
            VALUES (@Name, @Location, @TimeZone, @UnitId, @CreatedAt );";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery, new
            {
                entity.Name,
                entity.Location,
                entity.TimeZone,
                entity.UnitId,
                entity.CreatedAt
            });
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            List<Project> projects = new List<Project>();

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Project", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                projects.Add(DataReaderMappers.MapToProject(reader)!);
            }

            return projects;
        }

        public async Task<Project?> GetByIdAsync(int Id)
        {
            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Project WHERE Id = @Id", connection);
            cmd.Parameters.AddWithValue("@id", Id);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            Project project = DataReaderMappers.MapToProject(reader)!;

            return project;
        }

        public async Task UpdateAsync(Project entity)
        {
            var stringQuery = @"
            UPDATE Projects SET
                Name = @Name,
                Location = @Location,
                TimeZone = @TimeZone,
                UnitId = @UnitId
            WHERE Id = @Id;";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery, new
            {
                entity.Name,
                entity.Location,
                entity.TimeZone,
                entity.UnitId,
                entity.Id
            });
        }
    }
}
