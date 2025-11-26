using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAL.Service.Interface;

namespace PAL.Service
{
    public class SelectListService : ISelectListService
    {
        private readonly IStateService stateService;
        private readonly IDeviceTypeService deviceTypeService;
        private readonly IPriorityService priorityService;

        public SelectListService(IStateService stateService, IDeviceTypeService deviceTypeService, IPriorityService priorityService)
        {
            this.stateService = stateService;
            this.deviceTypeService = deviceTypeService;
            this.priorityService = priorityService;
        }

        public async Task<IEnumerable<SelectListItem>> GetStatesAsync()
        {
            var states = await stateService.GetAllAsync();
            var selectList = new List<SelectListItem>();
            foreach (var state in states)
            {
                selectList.Add(new SelectListItem { Value = state.Id.ToString(), Text = state.Name });
            }
            return selectList;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypesAsync()
        {
            var types = await deviceTypeService.GetAllAsync();
            var selectList = new List<SelectListItem>();
            foreach (var type in types)
            {
                selectList.Add(new SelectListItem { Value = type.Id.ToString(), Text = type.Name });
            }
            return selectList;
        }

        public async Task<IEnumerable<SelectListItem>> GetPrioritiesAsync()
        {
            var priorities = await priorityService.GetAllAsync();
            var selectList = new List<SelectListItem>();
            foreach (var priority in priorities)
            {
                selectList.Add(new SelectListItem { Value = priority.Id.ToString(), Text = priority.Name });
            }
            return selectList;
        }
    }
}
