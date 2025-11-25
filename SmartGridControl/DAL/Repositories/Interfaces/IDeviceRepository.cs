using DAL.Models;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IDeviceRepository : IUpdate<Device>,
        IGetAll<Device>,
        IGetById<Device>,
        IDelete
    {
    }
}
