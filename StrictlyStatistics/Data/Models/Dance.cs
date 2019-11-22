using System;
using SQLite;

namespace StrictlyStatistics
{
    [Table("Dances")]
    public class Dance
    {
        public int DanceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DegreeOfDifficulty { get; set; }
    }
}