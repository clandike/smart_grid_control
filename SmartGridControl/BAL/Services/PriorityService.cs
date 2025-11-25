using AutoMapper;
using BAL.DTO;
using BAL.Services.Interfaces;
using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class PriorityService : IPriorityService
    {
        private readonly IPriorityRepository priorityRepository;
        private readonly IMapper mapper;

        public PriorityService(IPriorityRepository priorityRepository, IMapper mapper)
        {
            this.priorityRepository = priorityRepository;
            this.mapper = mapper;
        }

        public async Task<PriorityDTO> GetByIdAsync(int id)
        {
            var entity = await priorityRepository.GetByIdAsync(id);
            var dto = mapper.Map<PriorityDTO>(entity);
            return dto;
        }

        public async Task<IEnumerable<PriorityDTO>> GetAllAsync()
        {
            var entities = await priorityRepository.GetAllAsync();
            IEnumerable<PriorityDTO> dtos = mapper.Map<IEnumerable<PriorityDTO>>(entities);
            return dtos;
        }
    }
}
