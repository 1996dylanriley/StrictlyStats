using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
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

            Sqlite();
            CreateHandlers();
        }

        public void Sqlite()
        {
            var docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var dbFile = Path.Combine(docFolder, "StrictlyStats.db");
            if (!System.IO.File.Exists(dbFile))
            {
                var stream = Resources.OpenRawResource(Resource.Raw.StrictlyStats);
                FileStream writeStream = new FileStream(dbFile, FileMode.OpenOrCreate, FileAccess.Write);
                ReadWriteStream(stream, writeStream);
            }
        }

        private void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int length = 256;
            byte[] buffer = new byte[length];
            int bytesRead = readStream.Read(buffer, 0, length);

            while(bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, length);
            }

            readStream.Close();
            writeStream.Close();
        }

        public void CreateHandlers()
        {
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var instructionsButton = FindViewById<Button>(Resource.Id.InstructionsButton);
            instructionsButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(Instructions)));

            var scoreEntryButton = FindViewById<Button>(Resource.Id.ScoreEntryButton);
            instructionsButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(ScoreEntry)));

            var weekRankingsButton = FindViewById<Button>(Resource.Id.WeeklyRankingsButton);
            instructionsButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(WeeklyRankings)));

            var couplesScoreBreakdownButton = FindViewById<Button>(Resource.Id.InstructionsButton);
            instructionsButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(Instructions)));

            var voteOffButton = FindViewById<Button>(Resource.Id.InstructionsButton);
            instructionsButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(Instructions)));

            var adminButton = FindViewById<Button>(Resource.Id.InstructionsButton);
            instructionsButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(Instructions)));
        }
    }
}