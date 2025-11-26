using BAL.DTO;
using BAL.Services;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAL.Models;

namespace PAL.Controllers
{
    [Controller]
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectService prjservice;
        private readonly IUnitService unitService;

        public ProjectController(IProjectService prjservice, IUnitService unitService, ILogger<ProjectController> logger)
        {
            this.unitService = unitService;
            this.prjservice = prjservice;
            _logger = logger;
        }

        [HttpGet()]
        public async Task<IActionResult> IndexAsync()
        {
            var prjs = await prjservice.GetAllAsync();
            List<ProjectViewModel> modelViews = new List<ProjectViewModel>();
            foreach (var prj in prjs)
            {
                var unitName = await unitService.GetByIdAsync(prj.UnitId);
                modelViews.Add(new ProjectViewModel()
                {
                    Id = prj.Id,
                    Name = prj.Name,
                    CreatedAt = prj.CreatedAt,
                    Location = prj.Location,
                    TimeZone = prj.TimeZone,
                    UnitId = prj.UnitId,
                    UnitName = unitName.Name,
                });
            }

            return View(modelViews.AsEnumerable());
        }

        [HttpGet()]
        public async Task<IActionResult> CreateAsync()
        {
            var list = new List<SelectListItem>();

            var units = await unitService.GetAllAsync();

            foreach (var unit in units)
            {
                list.Add(new SelectListItem() { Value = $"{unit.Id}", Text = unit.Name });
            }
            ;
            ViewBag.Units = list;
            return View(new ProjectDTO());
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync(ProjectDTO dto)
        {
            dto.CreatedAt = DateTime.Now;
            await prjservice.SaveAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await prjservice.GetByIdAsync(id);
            if (dto != null)
            {

                await prjservice.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
