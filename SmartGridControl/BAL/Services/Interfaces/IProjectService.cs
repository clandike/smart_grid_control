using BAL.DTO;
using BAL.Services.Interfaces.Basics;

namespace BAL.Services.Interfaces
{
    public interface IProjectService : 
        ISave<ProjectDTO>,
        IGetById<ProjectDTO>,
        IGetAll<ProjectDTO>,
        IDelete
    {
    }
}
