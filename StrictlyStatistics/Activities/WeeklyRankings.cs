﻿using System;
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
using StrictlyStatistics.UIComponents;

namespace StrictlyStatistics.Activities
{
    [Activity(Label = "WeeklyRankings")]
    public class WeeklyRankings : StrictlyStatsActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WeeklyRankings);

            InitialiseListView();
            InitialiseWeekInput();
        }

        void InitialiseListView()
        {
            var scores = Repo.GetAllScores().Where(x => x.WeekNumber == SelectedWeek);
            var couples = Repo.GetAllCouples().Where(x => scores.Select(y => y.CoupleID).Contains(x.CoupleID));

            var weekScores = new List<Tuple<string, int>>();
            foreach(var c in couples)
            {
                var coupleScore = scores.FirstOrDefault(x => x.CoupleID == c.CoupleID).ScoreValue;
                weekScores.Add(new Tuple<string, int>(c.CoupleName, coupleScore));
            }

            RankingListView.Initialise(this, weekScores, Resource.Id.scoresList);
        }
       
        void InitialiseWeekInput()
        {
            var weekInput = FindViewById<Spinner>(Resource.Id.weekRankingInput);
            var weeks = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var adapter = new ArrayAdapter<int>(this, Android.Resource.Layout.SimpleListItem1, weeks);
            weekInput.Adapter = adapter;

            weekInput.ItemSelected += (sender, e) =>
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