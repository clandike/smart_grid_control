using AutoMapper;
using BAL.DTO;
using BAL.Services.Interfaces;
using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class DeviceTypeService : IDeviceTypeService
    {
        private readonly IDeviceRepository deviceRepository;
        private readonly IMapper mapper;

        public DeviceTypeService(IDeviceRepository deviceRepository, IMapper mapper)
        {
            this.deviceRepository = deviceRepository;
            this.mapper = mapper;
        }

        public async Task<DeviceTypeDTO> GetByIdAsync(int id)
        {
            var entity = await deviceRepository.GetByIdAsync(id);
            var dto = mapper.Map<DeviceTypeDTO>(entity);
            return dto;
        }

        public async Task<IEnumerable<DeviceTypeDTO>> GetAllAsync()
        {
            var entities = await deviceRepository.GetAllAsync();
            IEnumerable<DeviceTypeDTO> dtos = mapper.Map<IEnumerable<DeviceTypeDTO>>(entities);
            return dtos;
        }
    }
}
