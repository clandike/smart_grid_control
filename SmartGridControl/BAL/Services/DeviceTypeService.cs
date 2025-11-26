using AutoMapper;
using BAL.DTO;
using BAL.Services.Interfaces;
using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class DeviceTypeService : IDeviceTypeService
    {
        private readonly IDeviceTypeRepository deviceTypeRepository;
        private readonly IMapper mapper;

        public DeviceTypeService(IDeviceTypeRepository deviceTypeRepository, IMapper mapper)
        {
            this.deviceTypeRepository = deviceTypeRepository;
            this.mapper = mapper;
        }

        public async Task<DeviceTypeDTO> GetByIdAsync(int id)
        {
            var entity = await deviceTypeRepository.GetByIdAsync(id);
            var dto = mapper.Map<DeviceTypeDTO>(entity);
            return dto;
        }

        public async Task<IEnumerable<DeviceTypeDTO>> GetAllAsync()
        {
            var entities = await deviceTypeRepository.GetAllAsync();
            IEnumerable<DeviceTypeDTO> dtos = mapper.Map<IEnumerable<DeviceTypeDTO>>(entities);
            return dtos;
        }
    }
}
