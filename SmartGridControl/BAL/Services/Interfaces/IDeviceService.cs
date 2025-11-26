using BAL.DTO;
using BAL.Services.Interfaces.Basics;

namespace BAL.Services.Interfaces
{
    public interface IDeviceService :
        ISave<DeviceDTO>,
        IGetById<DeviceDTO>,
        IGetAll<DeviceDTO>,
        IDelete
    {
    }
}
