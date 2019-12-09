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
using Autofac;

namespace StrictlyStatistics.Activities
{
    [Activity(Label = "ScoreEntry")]
    public class ScoreEntry : Activity
    {
        public IRepository Repo { get; set; }
        public Spinner DanceInput { get; set; }
        public Spinner WeekInput { get; set; }
        public NumberPicker ScoreInput { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Repo = MainApp.Container.Resolve<IRepository>();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ScoreEntry);
            InitialiseComponents();
        }

        public void InitialiseComponents()
        {
            InitialiseWeekInput();
            InitialiseDanceInput();
            CreateHandlers();
        }
        
        void InitialiseWeekInput()
        {
            int[] weeks = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            WeekInput = FindViewById<Spinner>(Resource.Id.weekInput);
            var adapter = new ArrayAdapter<int>(this, Android.Resource.Layout.SimpleListItem1, weeks);
            WeekInput.Adapter = adapter;
           
        }
        void InitialiseDanceInput()
        {
            DanceInput = FindViewById<Spinner>(Resource.Id.danceInput);
            var dances = Repo.GetAllDances().Select(x => x.Name).ToArray();
            DanceInput.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, dances);
        }

        void InitialiseScoreInput()
        {
            ScoreInput = FindViewById<NumberPicker>(Resource.Id.scoreInput);
            ScoreInput.MinValue = 0;
            ScoreInput.MaxValue = 40;
            //var dances = Repo.GetAllDances().Select(x => x.Name).ToArray();
            //DanceInput.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, dances);
        }


        void CreateHandlers()
        {
            //var instructionsButton = FindViewById<Button>(Resource.Id.InstructionsButton);
            //instructionsButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(Instructions));

            //WeekSpinner.ItemSelected += (sender, args) => Toast.MakeText(this, weeks[0], ToastLength.Short).Show(); 
        }
    }
}