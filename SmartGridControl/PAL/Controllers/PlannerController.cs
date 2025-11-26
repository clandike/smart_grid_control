using AutoMapper;
using BAL.Services.Interfaces;
using DAL.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using PAL.Models;
using Planner.Core;
using System.Text;

namespace WebApp.Controllers
{
    [Controller]
    public class PlannerController : Controller
    {
        private readonly IDeviceService deviceService;
        private readonly ISchedulerService schedulerService;
        private readonly IPriorityService priorityService;
        private readonly IStateService stateService;
        private readonly IDeviceTypeService deviceTypeService;

        private static readonly List<DecisionLogEntry> decisionLog = new();

        public PlannerController(IDeviceService deviceService, IMapper mapper, ISchedulerService schedulerService, IPriorityService priorityService, IStateService stateService, IDeviceTypeService deviceTypeService)
        {
            this.deviceService = deviceService;
            this.schedulerService = schedulerService;
            this.priorityService = priorityService;
            this.stateService = stateService;
            this.deviceTypeService = deviceTypeService;
        }

        // Основна сторінка планувальника
        [HttpGet("planner")]
        public async Task<IActionResult> Index(int projectId, int horizonMinutes = 120, int intervalMinutes = 60, decimal capacityLimit = 50m)
        {
            var devicesDto = await deviceService.GetAllAsync();

            List<DeviceViewModel> devices = new List<DeviceViewModel>();
            foreach (var item in devicesDto.Where(x => x.ProjectId == projectId))
            {
                var priority = await priorityService.GetByIdAsync(item.Priority);
                var type = await deviceTypeService.GetByIdAsync(item.TypeId);
                var state = await stateService.GetByIdAsync(item.StateId);


                devices.Add(new DeviceViewModel()
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

            int T = horizonMinutes / intervalMinutes;
            var prices = Enumerable.Range(0, T).Select(i => i % 2 == 0 ? 6m : 3m).ToList();
            var baseline = Enumerable.Range(0, T).Select(i => 10m).ToList();

            var schedule = schedulerService.BuildSchedule(projectId, devices, prices, baseline, intervalMinutes, capacityLimit);

            foreach (var interval in schedule.Intervals)
            {
                foreach (var d in interval.Decisions)
                {
                    decisionLog.Add(new DecisionLogEntry
                    {
                        Time = interval.Start,
                        DeviceName = d.DeviceName,
                        Action = d.On ? "ON" : "OFF",
                        ExpectedSaving = CalculateSaving(d, prices, baseline), // твоя функція
                        Reason = d.Notes
                    });
                }
            }

            return View(schedule);
        }


        [HttpPost("planner/override")]
        public IActionResult OverrideDevice(int projectId, bool forceOn)
        {
            decisionLog.Add(new DecisionLogEntry
            {
                Time = DateTime.UtcNow,
                DeviceName = $"Device {projectId}",
                Action = forceOn ? "ON" : "OFF",
                ExpectedSaving = 0,
                Reason = "Manual override"
            });
            TempData["Message"] = $"Device {projectId} overridden to {(forceOn ? "ON" : "OFF")}";

            return RedirectToAction("Index", new { projectId = projectId });
        }


        [HttpPost("planner/apply")]
        public IActionResult ApplySchedule(int projectId)
        {
            decisionLog.Add(new DecisionLogEntry
            {
                Time = DateTime.UtcNow,
                DeviceName = "Schedule",
                Action = "APPLIED",
                ExpectedSaving = decisionLog.Sum(x => x.ExpectedSaving), // можна підсумувати
                Reason = "Schedule applied"
            });

            ViewBag.ProjectId = projectId;
            TempData["Message"] = "Schedule applied successfully";
            return RedirectToAction("Index", new { projectId = projectId });
        }

        [HttpPost("planner/simulate")]
        public IActionResult SimulateStep(int projectId)
        {
            decisionLog.Add(new DecisionLogEntry
            {
                Time = DateTime.UtcNow,
                DeviceName = "Simulation",
                Action = "STEP",
                ExpectedSaving = 0,
                Reason = "Simulation step executed"
            });

            TempData["Message"] = "Simulation step executed";
            return RedirectToAction("Index", new { projectId = projectId });
        }

        [HttpGet("planner/export/html")]
        public IActionResult ExportHtml()
        {
            var totalSaving = decisionLog.Sum(x => x.ExpectedSaving);

            var sb = new StringBuilder();
            sb.Append("<h2>Planner Report</h2>");
            sb.Append($"<p>Total expected saving: {totalSaving} kWh</p>");
            sb.Append("<table border='1'><tr><th>Time</th><th>Device</th><th>Action</th><th>Saving</th><th>Reason</th></tr>");
            foreach (var log in decisionLog)
            {
                sb.Append($"<tr><td>{log.Time:HH:mm}</td><td>{log.DeviceName}</td><td>{log.Action}</td><td>{log.ExpectedSaving}</td><td>{log.Reason}</td></tr>");
            }
            sb.Append("</table>");
            return Content(sb.ToString(), "text/html");
        }

        [HttpGet("planner/export/pdf")]
        public IActionResult ExportPdf()
        {
            using var ms = new MemoryStream();
            var doc = new iTextSharp.text.Document(PageSize.A4);
            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            doc.Add(new Paragraph("Planner Report"));
            doc.Add(new Paragraph($"Generated: {DateTime.Now}"));

            var totalSaving = decisionLog.Sum(x => x.ExpectedSaving);
            doc.Add(new Paragraph($"Total expected saving: {totalSaving} kWh"));

            var table = new PdfPTable(5);
            table.AddCell("Time");
            table.AddCell("Device");
            table.AddCell("Action");
            table.AddCell("Saving");
            table.AddCell("Reason");

            foreach (var log in decisionLog)
            {
                table.AddCell(log.Time.ToString("HH:mm"));
                table.AddCell(log.DeviceName);
                table.AddCell(log.Action);
                table.AddCell(log.ExpectedSaving.ToString("0.##"));
                table.AddCell(log.Reason);
            }

            doc.Add(table);
            doc.Close();

            return File(ms.ToArray(), "application/pdf", "PlannerReport.pdf");
        }

        // Лог рішень
        [HttpGet("planner/log")]
        public IActionResult Log()
        {
            return View(decisionLog);
        }
        private decimal CalculateSaving(DeviceDecision d, List<decimal> prices, List<decimal> baseline)
        {
            int intervalIndex = 40; // потрібно мати це поле у DeviceDecision

            decimal price = prices.ElementAtOrDefault(intervalIndex);
            decimal baseLoad = baseline.ElementAtOrDefault(intervalIndex);

            decimal actualLoad = d.On ? d.RatedPower : 0;

            decimal saving = (baseLoad - actualLoad) * price;

            return saving;
        }
    }
}
