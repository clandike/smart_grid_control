using BAL.DTO;
using BAL.Services.Interfaces.Basics;

namespace BAL.Services.Interfaces
{
    public interface IUnitService : 
        IGetById<UnitDTO>,
        IGetAll<UnitDTO>
    {
    }
}
