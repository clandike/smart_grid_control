using PAL.Models;
using Planner.Core;

namespace PAL.Service
{
    public class SchedulerService : ISchedulerService
    {
        public ScheduleResult BuildSchedule(
            int projectId,
            IEnumerable<DeviceViewModel> devices,
            List<decimal> prices,
            List<decimal> baseline,
            int intervalMinutes,
            decimal capacityLimit)
        {
            var result = new ScheduleResult { ProjectId = projectId };
            int T = Math.Min(prices.Count, baseline.Count);

            decimal costBefore = 0, costAfter = 0;

            // Runtime state для min_on/min_off
            var runtime = devices.ToDictionary(d => d.Id, d => new DeviceRuntime { IsOn = d.Critical, MinutesInCurrentState = 0 });

            for (int t = 0; t < T; t++)
            {
                var start = DateTime.UtcNow.AddMinutes(t * intervalMinutes);
                var end = start.AddMinutes(intervalMinutes);
                var price = prices[t];
                var baseLoad = baseline[t];

                decimal currentLoad = baseLoad;
                var decisions = new List<DeviceDecision>();

                // Критичні пристрої завжди ON
                foreach (var d in devices.Where(x => x.Critical))
                {
                    decisions.Add(new DeviceDecision
                    {
                        DeviceId = d.Id,
                        DeviceName = d.Name,
                        TypeName = d.TypeName,
                        PriorityName = d.PriorityName,
                        StateName = d.StateName,
                        On = true,
                        RatedPower = d.RatedPower,
                        Notes = "Critical device"
                    });
                    currentLoad += d.RatedPower;
                    runtime[d.Id].IsOn = true;
                    runtime[d.Id].MinutesInCurrentState += intervalMinutes;
                }

                // Некритичні пристрої — heuristic
                var nonCritical = devices.Where(x => !x.Critical).OrderByDescending(x => x.Priority);

                foreach (var d in nonCritical)
                {
                    var state = runtime[d.Id];
                    bool canTurnOn = !state.IsOn && state.MinutesInCurrentState >= d.MinOffTime;
                    bool canTurnOff = state.IsOn && state.MinutesInCurrentState >= d.MinOnTime;

                    bool turnOn = false;

                    // Якщо інтервал дорогий → намагаємось вимкнути низький пріоритет
                    if (price > prices.Average())
                    {
                        if (state.IsOn && canTurnOff)
                        {
                            turnOn = false;
                            state.IsOn = false;
                            state.MinutesInCurrentState = 0;
                        }
                        else if (state.IsOn)
                        {
                            turnOn = true; // не можна вимкнути через min_on
                        }
                    }
                    else
                    {
                        // Дешевий інтервал → пробуємо вмикати
                        if (canTurnOn && currentLoad + d.RatedPower <= capacityLimit)
                        {
                            turnOn = true;
                            state.IsOn = true;
                            state.MinutesInCurrentState = 0;
                        }
                        else
                        {
                            turnOn = state.IsOn; // залишаємо як є
                        }
                    }

                    // Оновлення runtime
                    if (state.IsOn == turnOn)
                        state.MinutesInCurrentState += intervalMinutes;

                    decisions.Add(new DeviceDecision
                    {
                        DeviceId = d.Id,
                        DeviceName = d.Name,
                        TypeName = d.TypeName,
                        PriorityName = d.PriorityName,
                        StateName = d.StateName,
                        On = turnOn,
                        RatedPower = d.RatedPower,
                        Notes = turnOn ? "ON (heuristic)" : "OFF (heuristic)"
                    });

                    if (turnOn) currentLoad += d.RatedPower;
                }

                // Розрахунок вартості
                decimal naiveLoad = baseLoad + devices.Sum(d => d.RatedPower);
                costBefore += price * (naiveLoad * intervalMinutes / 60m);
                costAfter += price * (currentLoad * intervalMinutes / 60m);

                result.Intervals.Add(new IntervalResult
                {
                    Start = start,
                    End = end,
                    Price = price,
                    BaselineLoad = baseLoad,
                    TotalLoad = currentLoad,
                    Decisions = decisions
                });
            }

            result.CostBefore = costBefore;
            result.CostAfter = costAfter;
            return result;
        }
    }
}

