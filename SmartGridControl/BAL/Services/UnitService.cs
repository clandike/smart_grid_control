using AutoMapper;
using BAL.DTO;
using BAL.Services.Interfaces;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository unitRepository;
        private readonly IMapper mapper;

        public UnitService(IUnitRepository unitRepository, IMapper mapper)
        {
            this.unitRepository = unitRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UnitDTO>> GetAllAsync()
        {
            var entities = await unitRepository.GetAllAsync();
            IEnumerable<UnitDTO> dtos = mapper.Map<IEnumerable<UnitDTO>>(entities);
            return dtos;
        }

        public async Task<UnitDTO> GetByIdAsync(int id)
        {
            var entity = await unitRepository.GetByIdAsync(id);
            UnitDTO dto = mapper.Map<UnitDTO>(entity);
            return dto;
        }
    }
}
