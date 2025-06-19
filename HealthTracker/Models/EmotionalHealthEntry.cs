using SQLite;
using System;

namespace HealthTracker.Models
{
    [Table("EmotionalHealthEntries")]
    public class EmotionalHealthEntry : BaseModel
    {
        public int OverallMood { get; set; }
        public int StressLevel { get; set; }
        public int Anxiety { get; set; }
        public int EmotionalStability { get; set; }
        public int SocialConnection { get; set; }
        public int Gratitude { get; set; }
        public int Optimism { get; set; }
        public int EmotionalExpression { get; set; }
        public int SelfCompassion { get; set; }
        public int EmotionalRegulation { get; set; }
    }
}