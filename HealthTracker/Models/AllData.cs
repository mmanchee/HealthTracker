using System.Collections.Generic;

namespace HealthTracker.Models
{
    public class AllData
    {
        public List<MentalHealthEntry> MentalHealthEntries { get; set; }
        public List<PhysicalHealthEntry> PhysicalHealthEntries { get; set; }
        public List<EmotionalHealthEntry> EmotionalHealthEntries { get; set; }
        public List<JournalEntry> JournalEntries { get; set; }
        public List<TodoItem> TodoItems { get; set; }
    }
}
