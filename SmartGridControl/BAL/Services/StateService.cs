using AutoMapper;
using BAL.DTO;
using BAL.Services.Interfaces;
using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository stateRepository;
        private readonly IMapper mapper;

        public StateService(IStateRepository stateRepository, IMapper mapper)
        {
            this.stateRepository = stateRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<StateDTO>> GetAllAsync()
        {
            var entities = await stateRepository.GetAllAsync();
            IEnumerable<StateDTO> positions = mapper.Map<IEnumerable<StateDTO>>(entities);
            return positions;
        }

        public async Task<StateDTO> GetByIdAsync(int id)
        {
            var entity = await stateRepository.GetByIdAsync(id);
            StateDTO position = mapper.Map<StateDTO>(entity);
            return position;
        }
    }
}
