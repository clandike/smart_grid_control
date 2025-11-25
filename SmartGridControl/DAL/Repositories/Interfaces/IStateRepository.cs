using DAL.Models;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IStateRepository :
        IGetById<State>,
        IGetAll<State>
    {
    }
}
