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
using StrictlyStatistics.Data.Models;
using StrictlyStatistics.UIComponents;

namespace StrictlyStatistics.Activities
{
    [Activity(Label = "EditScore")]
    public class EditScore : StrictlyStatsActivity
    {
        List<Score> CoupleScores { get; set; }
        Score SelectedScore { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditScore);

            InitialiseButtons();
            CoupleSpinner.Initialise(this, Repo.GetAllCouples(), Resource.Id.editScoreCoupleSpinner, true, PopulateScoresAndWeeks);
            WeekSpinner.Initialise(this, weeks, Resource.Id.editScoreWeekSpinner, true, PopulateDances);
            DanceSpinner.Initialise(this, Repo.GetAllDances(), Resource.Id.editScoreDanceSpinner, true, PopulateScore);
            
        }

        public void InitialiseButtons()
        {

            var scoreValue = FindViewById<EditText>(Resource.Id.editScoreValue);
            scoreValue.TextChanged += (sender, args) =>
            {
                int score = 0;
                int.TryParse(args.Text.ToString(), out score);
                if (score != 0)
                    SelectedScore.ScoreValue = score;
            };

            var saveButton = FindViewById<Button>(Resource.Id.saveButton);
            saveButton.Click += (sender, args) =>
            {
                SelectedScore.CoupleID = Couple.CoupleID;
                SelectedScore.DanceID = Dance.DanceId;
                SelectedScore.WeekNumber = SelectedWeek;

                if (SelectedScore.CoupleID == 0 ||
                    SelectedScore.DanceID == 0 ||
                    SelectedScore.ScoreID == 0 ||
                    SelectedScore.ScoreValue == 0 ||
                    SelectedScore.WeekNumber == 0)                    
                    Alert.ShowAlertWithSingleButton(this, "Error", "All fields must be populated", "OK");
                else if (SelectedScore.ScoreValue > 40)
                    Alert.ShowAlertWithSingleButton(this, "Error", "Score value cannot be more than 40", "OK");
                else if (SelectedScore.ScoreValue < 0)
                    Alert.ShowAlertWithSingleButton(this, "Error", "Score value cannot be less than 0", "OK");
                else if (SelectedScore.ScoreID != 0)
                {
                    Repo.UpdateScore(SelectedScore);
                    Alert.ShowAlertWithSingleButton(this, "Success", "Score saved!", "OK");
                }
            };

            var deleteButton = FindViewById<Button>(Resource.Id.deleteButton);
            deleteButton.Click += (sender, args) =>
            {
                if (SelectedScore.ScoreID != 0)
                {
                    Repo.RemoveScore(SelectedScore);
                    Alert.ShowAlertWithSingleButton(this, "Success", "Score deleted", "OK");
                }
            };

            var cancelButton = FindViewById<Button>(Resource.Id.cancelButton);
            cancelButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(EditScore)));
        }

        public void PopulateScoresAndWeeks()
        {
            CoupleScores = Repo.GetAllScores().Where(x => x.CoupleID == Couple.CoupleID).ToList();
            var possibleWeeks = weeks.Where(x => CoupleScores.Select(z => z.WeekNumber).Contains(x)).ToList();
            WeekSpinner.Update(this, possibleWeeks, Resource.Id.editScoreWeekSpinner);
        }

        public void PopulateDances()
        {
            var possibleDanceIds = CoupleScores.Where(x => 
                                    x.CoupleID == Couple.CoupleID && 
                                    x.WeekNumber == SelectedWeek)
                                   .Select(x => x.DanceID)
                                   .ToList();

            var possibleDances = Repo.GetAllDances().Where(x => 
                                 possibleDanceIds.Contains(x.DanceId))
                                 .ToList();

            DanceSpinner.Update(this, possibleDances, Resource.Id.editScoreDanceSpinner);
        }

        public void PopulateScore()
        {
            SelectedScore = CoupleScores.Where(x => x.WeekNumber == SelectedWeek &&
                                               x.CoupleID == Couple.CoupleID)
                                               .FirstOrDefault() ?? new Score();

            var scoreValue = FindViewById<EditText>(Resource.Id.editScoreValue);
            scoreValue.Text = SelectedScore.ScoreValue.ToString();
        }
            
        
    }
}