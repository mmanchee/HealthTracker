using SQLite;
using System;

namespace HealthTracker.Models
{
    [Table("MentalHealthEntries")]
    public class MentalHealthEntry : BaseModel
    {
        public int FocusAndConcentration { get; set; }
        public int DecisionMaking { get; set; }
        public int Memory { get; set; }
        public int MentalClarity { get; set; }
        public int ProblemSolving { get; set; }
        public int MentalEnergy { get; set; }
        public int Learning { get; set; }
        public int Overwhelm { get; set; }
        public int MentalResilience { get; set; }
    }
}
