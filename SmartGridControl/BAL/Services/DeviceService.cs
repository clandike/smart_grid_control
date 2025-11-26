using AutoMapper;
using BAL.DTO;
using BAL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository deviceRepository;
        private readonly IMapper mapper;

        public DeviceService(IDeviceRepository deviceRepository, IMapper mapper)
        {
            this.deviceRepository = deviceRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<DeviceDTO>> GetAllAsync()
        {
            var entity = await deviceRepository.GetAllAsync();
            IEnumerable<DeviceDTO> dto = mapper.Map<IEnumerable<DeviceDTO>>(entity);
            return dto;
        }

        public async Task<DeviceDTO> GetByIdAsync(int id)
        {
            var entity = await deviceRepository.GetByIdAsync(id);
            DeviceDTO dto = mapper.Map<DeviceDTO>(entity);
            return dto;
        }

        public async Task SaveAsync(DeviceDTO dto)
        {
            var emp = await deviceRepository.GetByIdAsync(dto.Id);
            var entity = mapper.Map<Device>(dto);

            if (emp != null)
            {
                await deviceRepository.UpdateAsync(entity);
            }
            else
            {
                await deviceRepository.CreateAsync(entity);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await deviceRepository.DeleteAsync(id);
        }
    }
}
