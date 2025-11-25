using DAL.Models;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IProjectRepository :
       ICreate<Project>,
       IUpdate<Project>,
       IGetAll<Project>,
       IGetById<Project>,
       IDelete
    {
    }
}
