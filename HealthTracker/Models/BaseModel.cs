using SQLite;
using System;

namespace HealthTracker.Models
{
    public abstract class BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
