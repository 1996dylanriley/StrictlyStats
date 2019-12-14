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
using StrictlyStatistics.Adapters;

namespace StrictlyStatistics.Activities
{
    [Activity(Label = "WeeklyRankings")]
    public class WeeklyRankings : Activity
    {
        public IRepository Repo { get; set; }
        public ListView ScoresList { get; set; }
        public Spinner WeekInput { get; set; }
        public int SelectedWeek { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Repo = MainApp.Container.Resolve<IRepository>();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.WeeklyRankings);
            InitialiseComponents();
        }

        void InitialiseComponents()
        {
            InitialiseListView();
            InitialiseWeekInput();
        }

        void InitialiseListView()
        {
            ScoresList = FindViewById<ListView>(Resource.Id.scoresList);

            var scores = Repo.GetAllScores().Where(x => x.WeekNumber == SelectedWeek);
            var couples = Repo.GetCouples().Where(x => scores.Select(y => y.CoupleID).Contains(x.CoupleID));

            var rankings = new List<Tuple<string, int>>();
            foreach(var c in couples)
            {
                var coupleScore = scores.FirstOrDefault(x => x.CoupleID == c.CoupleID).ScoreValue;
                rankings.Add(new Tuple<string, int>(c.CelebrityFirstName + " and " + c.ProfessionalFirstName, coupleScore));
            }

            rankings.Sort((x,y) => y.Item2.CompareTo(x.Item2));

            for (int i = 0; i < rankings.Count; i++)
            {
                rankings[i] =  new Tuple<string, int>("#" + (i+1).ToString() + " " + rankings[i].Item1, rankings[i].Item2);
            }

            var adapter = new SimpleListItem2ListAdapter(this,  rankings);
            ScoresList.Adapter = adapter;
        }
       
        void InitialiseWeekInput()
        {
            WeekInput = FindViewById<Spinner>(Resource.Id.weekRankingInput);
            var weeks = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var adapter = new ArrayAdapter<int>(this, Android.Resource.Layout.SimpleListItem1, weeks);
            WeekInput.Adapter = adapter;

            WeekInput.ItemSelected += (sender, e) =>
            {
                Spinner spinner = (Spinner)sender;
                var selected = spinner.GetItemAtPosition(e.Position);
                string toast = string.Format("{0}", selected);
                Toast.MakeText(this, toast, ToastLength.Long).Show();
                SelectedWeek = (int)selected;
                InitialiseListView();
            };
        }
    }
}