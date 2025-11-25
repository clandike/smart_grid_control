using AutoMapper;
using BAL.DTO;
using DAL.Models;

namespace BAL.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectDTO, Project>();

            CreateMap<Device, DeviceDTO>();
            CreateMap<DeviceDTO, Device>();

            CreateMap<DeviceType, DeviceTypeDTO>();
            CreateMap<DeviceTypeDTO, DeviceType>();

            CreateMap<Unit, UnitDTO>();
            CreateMap<UnitDTO, Unit>();

            CreateMap<State, StateDTO>();
            CreateMap<StateDTO, State>();

            CreateMap<Priority, PriorityDTO>();
            CreateMap<PriorityDTO, Priority>();
        }
    }
}
