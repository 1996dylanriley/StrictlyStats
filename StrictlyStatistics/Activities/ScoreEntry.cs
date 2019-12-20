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
        List<Couple> PossibleCouples
        {
            get
            {
                if (SelectedWeek < 1)
                    return Repo.GetAllCouples();
                else
                {
                    var scoresForWeek = Repo.GetAllScores().Where(x => x.WeekNumber == SelectedWeek);
                    var possibleCouples = Repo.GetAllCouples().Where(x =>
                      (x.VotedOffWeekNumber >= SelectedWeek || x.VotedOffWeekNumber == null)
                       && !(scoresForWeek.Select(z => z.CoupleID).Contains(x.CoupleID))).ToList();
                    return possibleCouples;
                }

            }
        }
        int EnteredScore { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ScoreEntry);

            DanceSpinner.Initialise(this, Repo.GetAllDances().ToList(), Resource.Id.danceInput);
            CoupleSpinner.Initialise(this, Repo.GetAllCouples(), Resource.Id.coupleInput);
            InitialiseScoreInput();
            InitialiseWeekInput();
            InitialiseConfirmButton();
            InitialiseCancelButton();
            InitialiseStatsButton();
        }

        void InitialiseWeekInput()
        {
            Action updateInputsOnSelect = () =>
            {
                CoupleSpinner.Update(this, PossibleCouples, Resource.Id.coupleInput);
                if (PossibleCouples.Count == 0)
                    Alert.ShowAlertWithSingleButton(this, "Warning", "This week has already been populated", "OK");
                UpdateWeekStatsButtonVis();
            };
            WeekSpinner.Initialise(this, weeks, Resource.Id.weekInput, true, updateInputsOnSelect);
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
                var existingScoreForDance = Repo.GetAllScores().FirstOrDefault(x => x.CoupleID == Couple?.CoupleID && x.DanceID == Dance.DanceId);
                var existingScoreForWeek = Repo.GetAllScores().FirstOrDefault(x => x.CoupleID == Couple?.CoupleID && x.WeekNumber == SelectedWeek);

                if (SelectedWeek == 0 || Couple?.CoupleID == 0 || Dance?.DanceId == 0 || (EnteredScore <= 0 || EnteredScore > 40))
                    Alert.ShowAlertWithSingleButton(this, "Error", "All fields must be populated", "OK");
                else if(existingScoreForWeek != null)
                {
                    Action proceed = () => Save(existingScoreForWeek.ScoreID);
                    Action cancel = () => { };
                    Alert.ShowAlertWithTwoButtons(this, "Warning", "This couple already has an entry for the selected week", "Proceed", "Cancel", proceed, cancel);
                }
                else if (existingScoreForDance != null)
                {
                    Action proceed = () => Save(existingScoreForDance.ScoreID);
                    Action cancel = () => { };
                    Alert.ShowAlertWithTwoButtons(this, "Warning", "This couple already has an entry for the given dance", "Proceed", "Cancel", proceed, cancel);
                }
                else
                    Save();
                
            };         
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

        public void InitialiseStatsButton()
        {
            var btn = FindViewById<Button>(Resource.Id.weeksStatsButton);

            UpdateWeekStatsButtonVis();

            btn.Click += (sender, args) =>
            {
                var activity = new Intent(this, typeof(WeekStats));
                activity.PutExtra("week", SelectedWeek.ToString());
                StartActivity(activity);
            };
        }
        
        void UpdateWeekStatsButtonVis()
        {
            var btn = FindViewById<Button>(Resource.Id.weeksStatsButton); 
            if (PossibleCouples.Count() == 0 && SelectedWeek != 0)
                btn.Visibility = ViewStates.Visible;
            else
                btn.Visibility = ViewStates.Invisible;
        }

        void ResetPage()
        {
            CoupleSpinner.Update(this, PossibleCouples, Resource.Id.coupleInput);
            DanceSpinner.Update(this, Repo.GetAllDances().ToList(), Resource.Id.danceInput);
            EditText scoreInput = FindViewById<EditText>(Resource.Id.scoreInput);
            scoreInput.Text = "0";
            UpdateWeekStatsButtonVis();
        }
    }
}