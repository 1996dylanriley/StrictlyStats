using System.IO;
using Android.App;
using Android.OS;
using Android.Widget;
using StrictlyStatistics.Activities;

namespace StrictlyStatistics
{
    [Activity(Label = "Instructions")]
    public class Instructions : StrictlyStatsActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Instructions);

            var instructionsTextBox = FindViewById<TextView>(Resource.Id.instructionsText);

            string fileName = "Instructions.txt";
            string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(libraryPath, fileName);

            instructionsTextBox.Text = File.ReadAllText(path);
        }
    }
}