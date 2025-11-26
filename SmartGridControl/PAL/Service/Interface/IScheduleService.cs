using PAL.Models;

namespace Planner.Core
{
    public class DeviceRuntime
    {
        public bool IsOn { get; set; }
        public int MinutesInCurrentState { get; set; }
    }

    public class DeviceDecision
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string TypeName { get; set; }
        public string PriorityName { get; set; }
        public string StateName { get; set; }
        public bool On { get; set; }
        public decimal RatedPower { get; set; }
        public string Notes { get; set; }
    }

    public class IntervalResult
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public decimal Price { get; set; }
        public decimal BaselineLoad { get; set; }
        public decimal TotalLoad { get; set; }
        public List<DeviceDecision> Decisions { get; set; } = new();
    }

    public class ScheduleResult
    {
        public int ProjectId { get; set; }
        public List<IntervalResult> Intervals { get; set; } = new();
        public decimal CostBefore { get; set; }
        public decimal CostAfter { get; set; }
        public decimal Savings => CostBefore - CostAfter;
    }

    public interface ISchedulerService
    {
        ScheduleResult BuildSchedule(
            int projectId,
            IEnumerable<DeviceViewModel> devices,
            List<decimal> prices,
            List<decimal> baseline,
            int intervalMinutes,
            decimal capacityLimit);
    }
}