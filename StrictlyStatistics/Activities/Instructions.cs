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

namespace StrictlyStatistics
{
    [Activity(Label = "Instructions")]
    public class Instructions : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SQLiteRepository repo = new SQLiteRepository();

            // Create your application here
            SetContentView(Resource.Layout.Instructions);
            var testView = FindViewById<TextView>(Resource.Id.textView2);
            testView.Text = String.Join('\n', repo.GetAllDances().Select(x => x.Description).ToList());
        }
    }
}