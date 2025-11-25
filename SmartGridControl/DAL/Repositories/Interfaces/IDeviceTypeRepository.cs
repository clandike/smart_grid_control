using DAL.Models;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IDeviceTypeRepository : IGetById<DeviceType>, IGetAll<DeviceType>
    {
    }
}
