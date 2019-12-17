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
    [Activity(Label = "Admin")]
    public class Admin : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Admin);

            InitialiseComponents();
        }

        public void InitialiseComponents()
        {
            var editCoupleButton = FindViewById<Button>(Resource.Id.editCoupleButton);
            editCoupleButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(EditCouple)));

            var editDanceButton = FindViewById<Button>(Resource.Id.editDanceButton);
            editDanceButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(EditDance)));

            var editScoreButton = FindViewById<Button>(Resource.Id.editScoreButton);
            editScoreButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(EditScore)));
        }
    }
}