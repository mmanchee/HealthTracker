using SQLite;
using System;

namespace HealthTracker.Models
{
    [Table("TodoItems")]
    public class TodoItem : BaseModel
    {
        public string Summary { get; set; }
        public int Priority { get; set; } // 1-High, 5-Low
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompleteDate { get; set; }
    }
}
