using AutoMapper;
using BAL.DTO;
using BAL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository projectRepository;
        private readonly IMapper mapper;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            this.projectRepository = projectRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllAsync()
        {
            var entity = await projectRepository.GetAllAsync();
            IEnumerable<ProjectDTO> projects = mapper.Map<IEnumerable<ProjectDTO>>(entity);
            return projects;
        }

        public async Task<ProjectDTO> GetByIdAsync(int id)
        {
            var entity = await projectRepository.GetByIdAsync(id);
            ProjectDTO project = mapper.Map<ProjectDTO>(entity);
            return project;
        }

        public async Task SaveAsync(ProjectDTO dto)
        {
            var emp = await projectRepository.GetByIdAsync(dto.Id);
            var entity = mapper.Map<Project>(dto);

            if (emp != null)
            {
                await projectRepository.UpdateAsync(entity);
            }
            else
            {
                await projectRepository.CreateAsync(entity);
            }
        }
        public async Task DeleteAsync(int id)
        {
            await projectRepository.DeleteAsync(id);
        }
    }
}
