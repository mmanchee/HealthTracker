using SQLite;
using System;

namespace HealthTracker.Models
{
    [Table("PhysicalHealthEntries")]
    public class PhysicalHealthEntry : BaseModel
    {
        public int EnergyLevel { get; set; }
        public int SleepQuality { get; set; }
        public string PhysicalActivity { get; set; }
        public int PainDiscomfort { get; set; }
        public string Appetite { get; set; }
        public int Hydration { get; set; }
        public int PhysicalStrength { get; set; }
        public int Recovery { get; set; }
    }
}