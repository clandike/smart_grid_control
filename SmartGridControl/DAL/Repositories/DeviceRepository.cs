using DAL.Connection;
using DAL.Helpers;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public DeviceRepository(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task DeleteAsync(int id)
        {
            var stringQuery = $"DELETE FROM Device WHERE Id = @id";
            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery, new { id });
        }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            List<Device> devices = new List<Device>();

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Device", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                devices.Add(DataReaderMappers.MapToDevice(reader));
            }

            return devices;
        }

        public async Task<Device?> GetByIdAsync(int id)
        {
            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Device WHERE Id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            Device device = DataReaderMappers.MapToDevice(reader);

            return device;
        }

        public async Task UpdateAsync(Device entity)
        {
            var stringQuery = @"
            UPDATE Devices SET
                ProjectId = @ProjectId,
                Name = @Name,
                TypeId = @TypeId,
                RatedPower = @RatedPower,
                Priority = @Priority,
                Critical = @Critical,
                MinOnTime = @MinOnTime,
                MinOffTime = @MinOffTime,
                EstimatedEnergyPerCycle = @EstimatedEnergyPerCycle,
                FlexibilityStart = @FlexibilityStart,
                FlexibilityEnd = @FlexibilityEnd,
                StateId = @StateId
            WHERE Id = @Id;";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery, new
            {
                entity.ProjectId,
                entity.Name,
                entity.TypeId,
                entity.RatedPower,
                entity.Priority,
                entity.Critical,
                entity.MinOnTime,
                entity.MinOffTime,
                entity.EstimatedEnergyPerCycle,
                entity.FlexibilityStart,
                entity.FlexibilityEnd,
                entity.StateId,
                entity.Id,
            });
        }
    }
}
