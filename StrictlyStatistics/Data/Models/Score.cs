using System;
using SQLite;
using System;

namespace StrictlyStatistics.Data.Models
{
    [Table("Scores")]
    public class Score
    {
        [PrimaryKey] // SQLite attribute
        [AutoIncrement]
        [Column("ScoreID") ]
        public int ScoreID { get; set; }
        [Column("CoupleID")]
        public int CoupleID { get; set; }
        [Column("DanceID")]
        public int DanceID { get; set; }
        [Column("WeekNumber")]
        public int WeekNumber { get; set; }
        [Column("Score")]
        public int ScoreValue { get; set; }
    }
}