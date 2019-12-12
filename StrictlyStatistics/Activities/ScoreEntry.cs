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
        public Spinner WeekInput { get; set; }
        public Spinner DanceInput { get; set; }
        public Spinner CoupleInput { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Repo = MainApp.Container.Resolve<IRepository>();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ScoreEntry);
            InitialiseComponents();
            Scores = Repo.GetAllScores();
            Dances = Repo.GetAllDances();
            Couples = Repo.GetCouples();
        }

        public void InitialiseComponents()
        {
            InitialiseWeekInput();
            InitialiseDanceInput();
            InitialiseCoupleInput();
            InitialiseConfirmButton();
        }
        


        void InitialiseWeekInput()
        {
            WeekInput = FindViewById<Spinner>(Resource.Id.weekInput);
            var adapter = new ArrayAdapter<int>(this, Android.Resource.Layout.SimpleListItem1, weeks);
            WeekInput.Adapter = adapter;
            try
            {
                WeekInput.ItemSelected += (sender, e) =>
                {
                    try
                    {                        
                        Spinner spinner = (Spinner)sender;
                        var selected = spinner.GetItemAtPosition(e.Position);
                        string toast = string.Format("{0}", selected);
                        Toast.MakeText(this, toast, ToastLength.Long).Show();
                        Score.WeekNumber = (int)selected;
                    }
                    catch (Exception ex)
                    {
                        var x = ex.Message;
                    }
                };
            }
            catch (Exception e)
            {
                var x = e.Message;
            }            
        }

        void updateWeekInput()
        {
            var coupleVotedOffWeek = Couples.FirstOrDefault(x => x.CoupleID == Score.CoupleID)?.VotedOffWeekNumber ?? 13;
                
            weeks = weeks.Where(x => x < coupleVotedOffWeek).ToList();

            var adapter = new ArrayAdapter<int>(this, Android.Resource.Layout.SimpleListItem1, weeks);
            WeekInput.Adapter = adapter;

        }

        void InitialiseDanceInput()
        {
            DanceInput = FindViewById<Spinner>(Resource.Id.danceInput);
            var dances = Repo.GetAllDances().ToList();
            var danceNames = dances.Select(x => x.Name).ToList();
            danceNames.Insert(0, "Select dance");
            DanceInput.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, danceNames);
            try
            {
                DanceInput.ItemSelected += (sender, e) =>
                {
                    try
                    {
                        Spinner spinner = (Spinner)sender;
                        var selected = spinner.GetItemAtPosition(e.Position);
                        Score.DanceID = dances.FirstOrDefault(x => x.Name == selected.ToString()).DanceId;
                    }
                    catch (Exception ex)
                    {
                        var x = ex.Message;
                    }
                };
            }
            catch(Exception e)
            {
                var x = e.Message;
            }
        }

        void updateDancesInput()
        {
            var previousDances = Scores.Where(x => x.CoupleID == Score.CoupleID).Select(x => x.DanceID);
            var possibleDances = Dances.Where(x => !previousDances.Contains(x.DanceId)).ToList();

            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, possibleDances.Select(x => x.Name).ToList());
            DanceInput.Adapter = adapter;

        }

        void InitialiseCoupleInput()
        {
            CoupleInput = FindViewById<Spinner>(Resource.Id.coupleInput);
            var couples = Repo.GetCouples().ToList();
            var couplesNames = couples.Select(x => x.CelebrityFirstName + " " + x.ProfessionalFirstName).ToList();
            couplesNames.Insert(0, "Select couple");
            CoupleInput.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, couplesNames);
            try
            {
                CoupleInput.ItemSelected += (sender, e) =>
                {
                    try
                    {
                        Spinner spinner = (Spinner)sender;
                        var selected = spinner.GetItemAtPosition(e.Position);
                        Score.CoupleID = couples.FirstOrDefault(x => selected.ToString().Contains(x.CelebrityFirstName) 
                        && selected.ToString().Contains(x.ProfessionalFirstName)).CoupleID;
                        updateWeekInput();
                        updateDancesInput();
                    }
                    catch (Exception ex)
                    {
                        var x = ex.Message;
                    }
                };
            }
            catch (Exception e)
            {
                var x = e.Message;
            }
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

        void InitialiseConfirmButton()
        {
            Button button = FindViewById<Button>(Resource.Id.confirmButton);

            button.Click += (sender, args) =>
            {
                SetScoreValue();
                if (Score.WeekNumber == 0 || Score.CoupleID == 0 || Score.DanceID == 0 || (Score.ScoreValue < 0 || Score.ScoreValue > 40))
                    Alert.ShowAlertWithSingleButton(this, "Error", "All fields must be populated", "OK");
                else if(Scores.Any(x => x.CoupleID == Score.CoupleID && x.WeekNumber == Score.WeekNumber))
                {
                    Action proceed = () => Save(true);
                    Alert.ShowAlertWithTwoButtons(this, "Warning", "This couple already has an entry for the given week", "Proceed", "Cancel", proceed, null);
                }
                else
                {
                    Save();
                }
            };
        }

        public void Save(bool overriteExisting = false)
        {
            if (overriteExisting)
                Repo.OverwrtireScore(Score);
            else
                Repo.SaveCoupleScore(Score);
            Alert.ShowAlertWithSingleButton(this, "Sucess", "Successfully saved dance", "OK");
            ResetPage();
        }
        public void ResetPage()
        {
            Scores = Repo.GetAllScores();
            Score = new Score();
            weeks = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            InitialiseComponents();
        }
    }
}