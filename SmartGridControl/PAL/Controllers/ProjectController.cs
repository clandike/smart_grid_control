using BAL.DTO;
using BAL.Services;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PAL.Models;
using System.Diagnostics;

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
            return View(prjs);
        }

        [HttpGet()]
        public async Task<IActionResult> CreateAsync()
        {
            var units = await unitService.GetAllAsync();
            var unitsDict = units.ToDictionary(d => d.Id, d => d.Name);
            ViewData["Unit"] = " ";
            ViewBag.Units = unitsDict.Values
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();
            return View(new ProjectDTO());
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync(ProjectDTO dto)
        {
            dto.CreatedAt = DateTime.Now;
            await prjservice.SaveAsync(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}
