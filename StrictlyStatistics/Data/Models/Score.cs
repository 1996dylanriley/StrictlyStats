using System;
using SQLite;

namespace StrictlyStatistics.Data.Models
{
    [Table("Scores")]
    public class Scores
    {
        public int ScoreID { get; set; }
        public int CoupleID { get; set; }
        public int DanceID { get; set; }
        public int WeekNumber { get; set; }
        public int Score { get; set; }
    }
}