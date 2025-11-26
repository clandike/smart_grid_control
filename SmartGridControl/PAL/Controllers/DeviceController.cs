using Azure.Core;
using BAL.DTO;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PAL.Models;
using PAL.Service.Interface;

namespace PAL.Controllers
{
    [Controller]
    public class DeviceController : Controller
    {
        private readonly IDeviceService deviceService;
        private readonly ISelectListService selectListService;
        private readonly IPriorityService priorityService;
        private readonly IStateService stateService;
        private readonly IDeviceTypeService deviceTypeService;
        public DeviceController(IDeviceService deviceService, ISelectListService selectListService, IPriorityService priorityService, IStateService stateService, IDeviceTypeService deviceTypeService)
        {
            this.selectListService = selectListService;
            this.deviceService = deviceService;
            this.priorityService = priorityService;
            this.stateService = stateService;
            this.deviceTypeService = deviceTypeService;
        }

        [HttpGet()]
        public async Task<IActionResult> IndexAsync(int id)
        {
            var devices = await deviceService.GetAllAsync();
            List<DeviceViewModel> list = new List<DeviceViewModel>();
            foreach (var item in devices)
            {
                var priority = await priorityService.GetByIdAsync(item.Priority);
                var type = await deviceTypeService.GetByIdAsync(item.TypeId);
                var state = await stateService.GetByIdAsync(item.StateId);


                list.Add(new DeviceViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Critical = item.Critical,
                    EstimatedEnergyPerCycle = item.EstimatedEnergyPerCycle,
                    FlexibilityEnd = item.FlexibilityEnd,
                    FlexibilityStart = item.FlexibilityStart,
                    MinOffTime = item.MinOffTime,
                    MinOnTime = item.MinOnTime,
                    Priority = item.Priority,
                    ProjectId = item.ProjectId,
                    RatedPower = item.RatedPower,
                    StateId = item.StateId,
                    TypeId = item.TypeId,
                    PriorityName = priority.Name,
                    TypeName = type.Name,
                    StateName = state.Name,
                });
            }

            ViewBag.ProjectId = id;

            return View(list.Where(x => x.ProjectId == id));
        }

        // GET: /Device/Create
        public async Task<IActionResult> CreateAsync(int id)
        {
            var dto = new DeviceDTO() { ProjectId = id };
            ViewBag.States = await selectListService.GetStatesAsync();
            ViewBag.Types = await selectListService.GetTypesAsync();
            ViewBag.Priorities = await selectListService.GetPrioritiesAsync();

            return View("DeviceForm", dto);
        }

        // POST: /Device/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeviceDTO dto)
        {
            if (!ModelState.IsValid)
                return View("DeviceForm", dto);

            await deviceService.SaveAsync(dto);
            return RedirectToAction(nameof(Index), new { id = dto.ProjectId });
        }

        // GET: /Device/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var device = await deviceService.GetByIdAsync(id);
            if (device == null)
                return NotFound();

            ViewBag.States = await selectListService.GetStatesAsync();
            ViewBag.Types = await selectListService.GetTypesAsync();
            ViewBag.Priorities = await selectListService.GetPrioritiesAsync();

            return View("DeviceForm", device);
        }

        // POST: /Device/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DeviceDTO dto)
        {
            if (!ModelState.IsValid)
                return View("DeviceForm", dto);

            await deviceService.SaveAsync(dto);
            return RedirectToAction(nameof(Index), new { id = dto.ProjectId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await deviceService.GetByIdAsync(id);
            if (dto != null)
            {
                var projectId = dto.ProjectId;
                await deviceService.DeleteAsync(id);
                return RedirectToAction(nameof(Index), new { id = projectId });
            }
            return RedirectToPage("project");
        }
    }
}
