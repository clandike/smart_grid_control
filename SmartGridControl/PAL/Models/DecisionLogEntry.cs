namespace PAL.Models
{
    public class DecisionLogEntry
    {
        public DateTime Time { get; set; }
        public string DeviceName { get; set; }
        public string Action { get; set; } // ON/OFF
        public decimal ExpectedSaving { get; set; } // kWh або грн
        public string Reason { get; set; }
    }
}
