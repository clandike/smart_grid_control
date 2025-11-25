using DAL.Models;
using Microsoft.Data.SqlClient;

namespace DAL.Helpers
{
    public static class DataReaderMappers
    {
        public static Device MapToDevice(this SqlDataReader reader)
        {
            return new Device
            {
                Id = reader.GetFieldValueSafe<int>("Id"),
                ProjectId = reader.GetFieldValueSafe<int>("ProjectId"),
                Name = reader.GetFieldValueSafe<string>("Name"),
                TypeId = reader.GetFieldValueSafe<int>("TypeId"),
                RatedPower = reader.GetFieldValueSafe<decimal>("RatedPower"),
                Priority = reader.GetFieldValueSafe<int>("Priority"),
                Critical = reader.GetFieldValueSafe<bool>("Critical"),
                MinOnTime = reader.GetFieldValueSafe<int>("MinOnTime"),
                MinOffTime = reader.GetFieldValueSafe<int>("MinOffTime"),
                EstimatedEnergyPerCycle = reader.GetFieldValueSafe<decimal>("EstimatedEnergyPerCycle"),
                FlexibilityStart = TimeOnly.FromTimeSpan(reader.GetFieldValueSafe<TimeSpan>("FlexibilityStart")),
                FlexibilityEnd = TimeOnly.FromTimeSpan(reader.GetFieldValueSafe<TimeSpan>("FlexibilityEnd")),

                StateId = reader.GetFieldValueSafe<int>("StateId")
            };
        }

        public static DeviceType MapToDeviceType(this SqlDataReader reader)
        {
            return new DeviceType
            {
                Id = reader.GetFieldValueSafe<int>("Id"),
                Name = reader.GetFieldValueSafe<string>("Name"),
            };
        }

        public static Project MapToProject(this SqlDataReader reader)
        {
            return new Project
            {
                Id = reader.GetFieldValueSafe<int>("Id"),
                Name = reader.GetFieldValueSafe<string>("Name"),
                Location = reader.GetFieldValueSafe<string>("Location"),
                TimeZone = reader.GetFieldValueSafe<string>("TimeZone"),
                UnitId = reader.GetFieldValueSafe<int>("UnitId"),
                CreatedAt = reader.GetFieldValueSafe<DateTime>("CreatedAt")
            };
        }

        public static Unit MapToUnit(this SqlDataReader reader)
        {
            return new Unit
            {
                Id = reader.GetFieldValueSafe<int>("Id"),
                Name = reader.GetFieldValueSafe<string>("Name"),
            };
        }

        public static State MapToState(this SqlDataReader reader)
        {
            return new State
            {
                Id = reader.GetFieldValueSafe<int>("Id"),
                Name = reader.GetFieldValueSafe<string>("Name"),
            };
        }

        public static Priority MapToPriority(this SqlDataReader reader)
        {
            return new Priority
            {
                Id = reader.GetFieldValueSafe<int>("Id"),
                Name = reader.GetFieldValueSafe<string>("Name"),
            };
        }
    }
}
