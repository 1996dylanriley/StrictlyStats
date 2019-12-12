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
    public class ScoreEntry : Activity
    {
        public IRepository Repo { get; set; }
        Score Score { get; set; } = new Score();
        List<Score> Scores { get; set; }
        List<Dance> Dances { get; set; }
        List<Couple> Couples { get; set; }
        List<int> weeks = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        Spinner WeekInput { get; set; }
        Spinner DanceInput { get; set; }
        Spinner CoupleInput { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Repo = MainApp.Container.Resolve<IRepository>();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ScoreEntry);
            InitialiseComponents(true);
            Scores = Repo.GetAllScores();
            Dances = Repo.GetAllDances();
            Couples = Repo.GetCouples();
        }

        void InitialiseComponents(bool addEventhandlers)
        {
            InitialiseWeekInput(addEventhandlers);
            InitialiseDanceInput(addEventhandlers);
            InitialiseCoupleInput(addEventhandlers);
            InitialiseConfirmButton(addEventhandlers);
            InitialiseCancelButton(addEventhandlers);
        }
        


        void InitialiseWeekInput(bool addEventhandlers)
        {
            WeekInput = FindViewById<Spinner>(Resource.Id.weekInput);
            var adapter = new ArrayAdapter<int>(this, Android.Resource.Layout.SimpleListItem1, weeks);
            WeekInput.Adapter = adapter;
            if(addEventhandlers)
            {
                WeekInput.ItemSelected += (sender, e) =>
                {                       
                        Spinner spinner = (Spinner)sender;
                        var selected = spinner.GetItemAtPosition(e.Position);
                        string toast = string.Format("{0}", selected);
                        Toast.MakeText(this, toast, ToastLength.Long).Show();
                        Score.WeekNumber = (int)selected;
                  
                };
            }   
        }

        void InitialiseDanceInput(bool addEventhandlers)
        {
            DanceInput = FindViewById<Spinner>(Resource.Id.danceInput);
            var dances = Repo.GetAllDances().ToList();
            var danceNames = dances.Select(x => x.Name).ToList();
            danceNames.Insert(0, "Select dance");
            DanceInput.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, danceNames);

            if (addEventhandlers)
            {
                DanceInput.ItemSelected += (sender, e) =>
                {
                    Spinner spinner = (Spinner)sender;
                    var selected = spinner.GetItemAtPosition(e.Position);
                    Score.DanceID = dances.FirstOrDefault(x => x.Name == selected.ToString())?.DanceId ?? 0;

                };
            }            
        }

        void InitialiseCoupleInput(bool addEventhandlers)
        {
            CoupleInput = FindViewById<Spinner>(Resource.Id.coupleInput);
            var couples = Repo.GetCouples().ToList();
            var couplesNames = couples.Select(x => x.CelebrityFirstName + " " + x.ProfessionalFirstName).ToList();
            couplesNames.Insert(0, "Select couple");
            CoupleInput.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, couplesNames);
            if(addEventhandlers)
            {
                CoupleInput.ItemSelected += (sender, e) =>
                {
                    Spinner spinner = (Spinner)sender;
                    var selected = spinner.GetItemAtPosition(e.Position);
                    Score.CoupleID = couples.FirstOrDefault(x => selected.ToString().Contains(x.CelebrityFirstName) 
                    && selected.ToString().Contains(x.ProfessionalFirstName))?.CoupleID ?? 0;
                    UpdateWeekInput();
                    UpdateDancesInput();
                    
                };
            }
        }

        void InitialiseCancelButton(bool addEventhandlers)
        {
            Button button = FindViewById<Button>(Resource.Id.cancelButton);

            if (addEventhandlers)
            {
                button.Click += (sender, args) =>
                {
                    ResetPage();
                };
            }
        }

        void InitialiseConfirmButton(bool addEventhandlers)
        {
            Button button = FindViewById<Button>(Resource.Id.confirmButton);

            if (addEventhandlers)
            {
                button.Click += (sender, args) =>
                {
                    SetScoreValue();
                    if (Score.WeekNumber == 0 || Score.CoupleID == 0 || Score.DanceID == 0 || (Score.ScoreValue < 0 || Score.ScoreValue > 40))
                        Alert.ShowAlertWithSingleButton(this, "Error", "All fields must be populated", "OK");
                    else if (Scores.Any(x => x.CoupleID == Score.CoupleID && x.WeekNumber == Score.WeekNumber))
                    {
                        Action proceed = () => Save(true);
                        Action cancel = () => { };
                        Alert.ShowAlertWithTwoButtons(this, "Warning", "This couple already has an entry for the given week", "Proceed", "Cancel", proceed, cancel);
                    }
                    else
                    {
                        Save();
                    }
                };
            }            
        }
        
        void UpdateWeekInput()
        {
            var coupleVotedOffWeek = Couples.FirstOrDefault(x => x.CoupleID == Score.CoupleID)?.VotedOffWeekNumber ?? 0;
            if (coupleVotedOffWeek == 0)
                coupleVotedOffWeek = 13;

            weeks = weeks.Where(x => x < coupleVotedOffWeek).ToList();

            var adapter = new ArrayAdapter<int>(this, Android.Resource.Layout.SimpleListItem1, weeks);
            WeekInput.Adapter = adapter;

        }

        void UpdateDancesInput()
        {
            var previousDances = Scores.Where(x => x.CoupleID == Score.CoupleID).Select(x => x.DanceID).ToList();

            List<Dance> possibleDances;
            if (previousDances.Count == 0)
                possibleDances = Dances;
            else
                possibleDances = Dances.Where(x => !previousDances.Contains(x.DanceId)).ToList();

            var danceNames = possibleDances.Select(x => x.Name).ToList();
            danceNames.Insert(0, "Select dance");

            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, danceNames);
            DanceInput.Adapter = adapter;

        }

        void SetScoreValue()
        {
            EditText scoreInput = FindViewById<EditText>(Resource.Id.scoreInput);
            var input = int.Parse(scoreInput.Text);
            if (input > 40 || input == 0)
            {
                scoreInput.SetError("Score cannot be more than 40 or less than 0", null);
            }

            Score.ScoreValue = input;
        }

        void Save(bool overriteExisting = false)
        {
            if (overriteExisting)
                Repo.OverwrtireScore(Score);
            else
                Repo.SaveCoupleScore(Score);
            Alert.ShowAlertWithSingleButton(this, "Sucess", "Successfully saved dance", "OK");
            ResetPage();
        }
        void ResetPage()
        {
            Scores = Repo.GetAllScores();
            Score = new Score();
            weeks = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            InitialiseComponents(false);
        }
    }
}