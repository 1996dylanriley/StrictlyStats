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

namespace StrictlyStatistics.Activities
{
    [Activity(Label = "WeekStats")]
    public class WeekStats : StrictlyStatsActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WeekStats);

            int weekNumber = int.Parse(Intent.GetStringExtra("week"));

            var title = FindViewById<TextView>(Resource.Id.WeekStatsTitle);
            title.Text = "Statistics for week " + weekNumber.ToString();

            var scores = Repo.GetAllScores().Where(x => x.WeekNumber == weekNumber).Select(x => x.ScoreValue).OrderByDescending(x => x);
            var topScoreText = "Top score: " + scores.First().ToString();
            var bottomScoreText = "Bottom score: " + scores.Last().ToString();
            var avgScoreText = "Average score: " + ((int)scores.Average()).ToString();

            var statsTextBox = FindViewById<TextView>(Resource.Id.weekStatsText);
            statsTextBox.Text = string.Join('\n', new string[] { topScoreText, bottomScoreText, avgScoreText });
        }
    }
}