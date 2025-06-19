using SQLite;
using System;

namespace HealthTracker.Models
{
    [Table("JournalEntries")]
    public class JournalEntry : BaseModel
    {
        public string Content { get; set; }
    }
}
