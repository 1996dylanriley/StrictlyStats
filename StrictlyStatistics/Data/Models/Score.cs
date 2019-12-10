using System;
using SQLite;
using System;

namespace StrictlyStatistics.Data.Models
{
    [Table("Scores")]
    public class Score
    {
        public int ScoreID { get; set; }
        public int CoupleID { get; set; }
        public int DanceID { get; set; }
        public int WeekNumber { get; set; }
        [Column("Score")]
        public int ScoreValue { get; set; }
    }
}