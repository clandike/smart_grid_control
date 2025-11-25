using BAL.DTO;
using BAL.Services.Interfaces.Basics;

namespace BAL.Services.Interfaces
{
    public interface IPriorityService : IGetById<PriorityDTO>,
        IGetAll<PriorityDTO>
    {
    }
}
