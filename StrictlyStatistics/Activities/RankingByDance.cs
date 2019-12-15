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
    [Activity(Label = "RankingByDance")]
    public class RankingByDance : StrictlyStatsActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RankingByDance);
                        
            DanceSpinner.Initialise(this, Repo.GetAllDances(), Resource.Id.rankingByDanceSpinner, true, UpdateDanceRankingsListView);
        }

        void UpdateDanceRankingsListView()
        {
            var scores = Repo.GetAllScores().Where(x => x.DanceID == Dance?.DanceId).ToList();
            var couplesInScores = Repo.GetAllCouples().Where(x => scores.Select(z => z.ScoreID).Contains(x.CoupleID)).ToList();

            var couplesDanceScores = new List<Tuple<string, int>>();
            foreach (var c in couplesInScores)
            {
                var couplesDanceScore = scores.FirstOrDefault(x => x.CoupleID == c.CoupleID).ScoreValue;
                couplesDanceScores.Add(new Tuple<string, int>(c.CoupleName, couplesDanceScore));
            }

            RankingListView.Initialise(this, couplesDanceScores, Resource.Id.rankingByDanceListView, "This dance has not been performed by any couple yet");
        }

    }
}