namespace BAL.DTO
{
    public class DeviceDTO
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string Name { get; set; }

        public int TypeId { get; set; }

        public decimal RatedPower { get; set; }

        public int Priority { get; set; }

        public bool Critical { get; set; }

        public int MinOnTime { get; set; }

        public int MinOffTime { get; set; }

        public decimal EstimatedEnergyPerCycle { get; set; }

        public TimeOnly FlexibilityStart { get; set; }

        public TimeOnly FlexibilityEnd { get; set; }

        public int StateId { get; set; }
    }
}
