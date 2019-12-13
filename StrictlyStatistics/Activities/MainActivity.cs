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
            //Check this tutorial out
            //https://hofmadresu.com/2018/04/29/todo-xamarin-native-android.html
            
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            CreateHandlers();
        }

        public void CreateHandlers()
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

            var adminButton = FindViewById<Button>(Resource.Id.AdminButton);
            instructionsButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(Instructions)));
        }
    }
}