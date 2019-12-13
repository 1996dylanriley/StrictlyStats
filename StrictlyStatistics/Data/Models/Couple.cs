using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace StrictlyStatistics.Data.Models
{
    [Table("Couples")]
    public class Couple
    {
            [PrimaryKey] // SQLite attribute
            [AutoIncrement]
            public int CoupleID { get; set; }
            public string CelebrityFirstName { get; set; }
            public string CelebrityLastName { get; set; }
            public string ProfessionalFirstName { get; set; }
            public string ProfessionalLastName { get; set; }
            public int CelebrityStarRating { get; set; }
            public int? VotedOffWeekNumber { get; set; }
    }
}