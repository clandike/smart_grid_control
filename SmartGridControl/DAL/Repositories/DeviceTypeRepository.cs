using DAL.Connection;
using DAL.Helpers;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories
{
    public class DeviceTypeRepository : IDeviceTypeRepository
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public DeviceTypeRepository(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<DeviceType>> GetAllAsync()
        {
            List<DeviceType> deviceTypes = new List<DeviceType>();

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM DeviceType", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                deviceTypes.Add(DataReaderMappers.MapToDeviceType(reader));
            }

            return deviceTypes;
        }

        public async Task<DeviceType> GetByIdAsync(int id)
        {
            DeviceType company;

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM DeviceType WHERE Id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            DeviceType deviceType = DataReaderMappers.MapToDeviceType(reader);

            return deviceType;
        }
    }
}