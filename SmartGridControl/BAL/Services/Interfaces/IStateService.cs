using BAL.DTO;
using BAL.Services.Interfaces.Basics;

namespace BAL.Services.Interfaces
{
    public interface IStateService :
        IGetById<StateDTO>,
        IGetAll<StateDTO>
    {
    }
}
