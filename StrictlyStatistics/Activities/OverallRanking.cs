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
using StrictlyStatistics.UIComponents;

namespace StrictlyStatistics.Activities
{
    [Activity(Label = "OverallRanking")]
    public class OverallRanking : StrictlyStatsActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OverallRanking);

            InitialiseOverallRankingListView();
        }

        void InitialiseOverallRankingListView()
        {
            var averageScores = new List<Tuple<string, int>>();
            foreach (var c in Repo.GetAllCouples())
            {
                var averageScore = Repo.GetAllScores().Where(x => x.CoupleID == c.CoupleID).Average(x => x.ScoreValue);
                averageScores.Add(new Tuple<string, int>(c.CoupleName, (int)averageScore));
            }

            RankingListView.Initialise(this, averageScores, Resource.Id.overallRankingListview, "The competition has not started yet.");
        }
    }
}