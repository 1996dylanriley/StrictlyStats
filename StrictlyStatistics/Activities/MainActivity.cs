using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Android.Content;
using StrictlyStatistics.Activities;
using System.IO;


namespace StrictlyStatistics
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            InitialiseComponents();
        }

        public void InitialiseComponents()
        {
            var instructionsButton = FindViewById<Button>(Resource.Id.InstructionsButton);
            instructionsButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(Instructions)));

            var scoreEntryButton = FindViewById<Button>(Resource.Id.ScoreEntryButton);
            scoreEntryButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(ScoreEntry)));

            var weekRankingsButton = FindViewById<Button>(Resource.Id.WeeklyRankingsButton);
            weekRankingsButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(WeeklyRankings)));

            var couplesScoreBreakdownButton = FindViewById<Button>(Resource.Id.CouplesScoreBreakdownButton);
            couplesScoreBreakdownButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(CoupleScoreBreakdown)));

            var voteOffButton = FindViewById<Button>(Resource.Id.VoteButton);
            voteOffButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(VoteOff)));

            var rankingByDanceButton = FindViewById<Button>(Resource.Id.rankingByDanceButton);
            rankingByDanceButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(RankingByDance)));

            var overAllRankingButton = FindViewById<Button>(Resource.Id.overallRankingButton);
            overAllRankingButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(OverallRanking)));

            var adminButton = FindViewById<Button>(Resource.Id.AdminButton);
            adminButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(Admin)));
        }
    }
}