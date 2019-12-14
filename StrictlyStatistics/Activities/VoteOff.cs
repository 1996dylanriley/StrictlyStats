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
using StrictlyStatistics.Data.Models;
using StrictlyStatistics.UIComponents;

namespace StrictlyStatistics.Activities
{
    [Activity(Label = "VoteOff")]
    public class VoteOff : Activity
    {
        public IRepository Repo { get; set; }
        Spinner CoupleInput { get; set; }
        Couple Couple { get; set; }
        int SelectedWeek { get; set; }
        //dupe
        Spinner WeekInput { get; set; }
        //dupe
        List<int> weeks = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Repo = MainApp.Container.Resolve<IRepository>();
            SetContentView(Resource.Layout.VoteOff);
            InitialiseComponents();
        }

        void InitialiseComponents()
        {
            InitialiseCoupleInput();
            InitialiseVoteButton();
            InitialiseWeekInput();
        }

        //Dupe
        void InitialiseCoupleInput()
        {
            CoupleInput = FindViewById<Spinner>(Resource.Id.voteCoupleSpinner);
            var couples = Repo.GetCouples().ToList();
            var couplesNames = couples.Select(x => x.CoupleName).ToList();
            couplesNames.Insert(0, "Select couple");
            CoupleInput.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, couplesNames);
            CoupleInput.ItemSelected += (sender, e) =>
            {
                Spinner spinner = (Spinner)sender;
                var selected = spinner.GetItemAtPosition(e.Position);
                Couple = couples.FirstOrDefault(x => selected.ToString().Contains(x.CoupleName));
            };
        }

        void InitialiseVoteButton()
        {
            Button button = FindViewById<Button>(Resource.Id.voteOffButton);
            button.Click += (sender, args) =>
            {                    
                if(Couple == null)
                {
                    Alert.ShowAlertWithSingleButton(this, "Error", "You must select a couple", "Ok");
                }
                else if (SelectedWeek == 0)
                {
                    Alert.ShowAlertWithSingleButton(this, "Error", "Week cannot be 0!", "Ok");
                }
                else if (Repo.GetCouple(Couple.CoupleID).VotedOffWeekNumber == null)
                {
                    Save();
                }
                else
                {
                    Action proceed = () => Save();
                    Action cancel = () => { };
                    Alert.ShowAlertWithTwoButtons(this, "Warning", "This couple already has an entry for the given week", "Proceed", "Cancel", proceed, cancel);
                }
            };
        }

        //Dupe diff weekinput should i make this a param?
        void InitialiseWeekInput()
        {
            WeekInput = FindViewById<Spinner>(Resource.Id.voteOffWeekSpinner);
            var adapter = new ArrayAdapter<int>(this, Android.Resource.Layout.SimpleListItem1, weeks);
            WeekInput.Adapter = adapter;
            WeekInput.ItemSelected += (sender, e) =>
            {
                Spinner spinner = (Spinner)sender;
                var selected = spinner.GetItemAtPosition(e.Position);
                string toast = string.Format("{0}", selected);
                Toast.MakeText(this, toast, ToastLength.Long).Show();
                SelectedWeek = (int)selected;
            };
        }

        //Dupe
        void Save()
        {
            Couple.VotedOffWeekNumber = SelectedWeek;
            Repo.UpdateCouple(Couple);
            Alert.ShowAlertWithSingleButton(this, "Sucess", "Successfully saved dance", "OK");
        }
    }
}