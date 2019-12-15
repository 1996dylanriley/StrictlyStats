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
    [Activity(Label = "ScoreEntry")]
    public class ScoreEntry : StrictlyStatsActivity
    {
        List<int> PossibleWeeks { get; set; }
        int EnteredScore { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ScoreEntry);

            InitialiseScoreInput();
            WeekSpinner.Initialise(this, weeks, Resource.Id.weekInput);
            DanceSpinner.Initialise(this, Repo.GetAllDances().ToList(), Resource.Id.danceInput);
            InitialiseCoupleInput();
            InitialiseConfirmButton();
            InitialiseCancelButton();
        }    
        void InitialiseCoupleInput()
        {
            Action updateInputsOnSelect = () =>
            {
                UpdatePossibleWeeks();
                WeekSpinner.Update(this, PossibleWeeks, Resource.Id.weekInput);

                UpdatePossibleDancesInput();
            };
            CoupleSpinner.Initialise(this, Repo.GetAllCouples().ToList(), Resource.Id.coupleInput, true, updateInputsOnSelect);
        }

        void InitialiseCancelButton()
        {
            Button button = FindViewById<Button>(Resource.Id.cancelButton);
            button.Click += (sender, args) => ResetPage();
        }

        void InitialiseConfirmButton()
        {
            Button button = FindViewById<Button>(Resource.Id.confirmButton);

            button.Click += (sender, args) =>
            {                    
                var existingScore = Repo.GetAllScores().FirstOrDefault(x => x.CoupleID == Couple?.CoupleID && x.WeekNumber == SelectedWeek);
                if (SelectedWeek == 0 || Couple?.CoupleID == 0 || Dance?.DanceId == 0 || (EnteredScore <= 0 || EnteredScore > 40))
                    Alert.ShowAlertWithSingleButton(this, "Error", "All fields must be populated", "OK");
                else if (existingScore != null)
                {
                    Action proceed = () => Save(existingScore.ScoreID);
                    Action cancel = () => { };
                    Alert.ShowAlertWithTwoButtons(this, "Warning", "This couple already has an entry for the given week", "Proceed", "Cancel", proceed, cancel);
                }
                else
                    Save();
            };         
        }

        void UpdatePossibleWeeks()
        {
            PossibleWeeks = weeks.Where(x => x < Couple?.VotedOffWeekNumber || Couple?.VotedOffWeekNumber == null || Couple == null).ToList();
        }

        void UpdatePossibleDancesInput()
        {
            var previousDances = Repo.GetAllScores().Where(x => x.CoupleID == Couple?.CoupleID).Select(x => x.DanceID).ToList();
            List<Dance> possibleDances = Repo.GetAllDances().ToList();
            possibleDances = Repo.GetAllDances().Where(x => !previousDances.Contains(x.DanceId)).ToList();
            DanceSpinner.Update(this, possibleDances, Resource.Id.danceInput);
        }

        void InitialiseScoreInput()
        {
            EditText scoreInput = FindViewById<EditText>(Resource.Id.scoreInput);
            scoreInput.TextChanged += (sender, args) =>
            {
                var input = int.Parse(scoreInput.Text);
                if (input > 40 || input == 0)
                {
                    scoreInput.SetError("Score cannot be more than 40 or less than 0", null);
                }
                EnteredScore = input;
            };            
        }

        void Save(int scoreId = 0)
        {
            var score = new Score()
            {
                ScoreID = scoreId,
                CoupleID = Couple.CoupleID,
                DanceID = Dance.DanceId,
                WeekNumber = SelectedWeek,
                ScoreValue = EnteredScore
            };

            if (scoreId != 0)
                Repo.UpdateScore(score);
            else
                Repo.SaveCoupleScore(score);

            Alert.ShowAlertWithSingleButton(this, "Sucess", "Successfully saved dance", "OK");
            ResetPage();
        }

        void ResetPage()
        {
            CoupleSpinner.Update(this, Repo.GetAllCouples(), Resource.Id.coupleInput);
            WeekSpinner.Update(this, weeks, Resource.Id.weekInput);
            DanceSpinner.Update(this, Repo.GetAllDances().ToList(), Resource.Id.danceInput);
            EditText scoreInput = FindViewById<EditText>(Resource.Id.scoreInput);
            scoreInput.Text = "0";
        }
    }
}