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
    public class VoteOff : StrictlyStatsActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.VoteOff);

            CoupleSpinner.Initialise(this, Repo.GetAllCouples().ToList(), Resource.Id.voteCoupleSpinner);
            WeekSpinner.Initialise(this, weeks, Resource.Id.voteOffWeekSpinner);
            InitialiseVoteButton();
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
                else if (Couple.VotedOffWeekNumber == null)
                {
                    Save();
                }
                else
                {
                    Action proceed = () => Save();
                    Action cancel = () => { };
                    Alert.ShowAlertWithTwoButtons(this, "Warning", "This couple has already been voted off", "Overwrite", "Cancel", proceed, cancel);
                }
            };
        }

        void Save()
        {
            Couple.VotedOffWeekNumber = SelectedWeek;
            Repo.UpdateCouple(Couple);
            Alert.ShowAlertWithSingleButton(this, "Sucess", "Successfully saved vote", "OK");
        }
    }
}