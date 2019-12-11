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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Repo = MainApp.Container.Resolve<IRepository>();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ScoreEntry);
            InitialiseComponents();
            Scores = Repo.GetAllScores();
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
            int[] weeks = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            Spinner weekInput = FindViewById<Spinner>(Resource.Id.weekInput);
            var adapter = new ArrayAdapter<int>(this, Android.Resource.Layout.SimpleListItem1, weeks);
            weekInput.Adapter = adapter;
            try
            {
                weekInput.ItemSelected += (sender, e) =>
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
        void InitialiseDanceInput()
        {
            Spinner danceInput = FindViewById<Spinner>(Resource.Id.danceInput);
            var dances = Repo.GetAllDances().ToList();
            var danceNames = dances.Select(x => x.Name).ToList();
            danceNames.Insert(0, "Select dance");
            danceInput.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, danceNames);
            try
            {
                danceInput.ItemSelected += (sender, e) =>
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

        void InitialiseCoupleInput()
        {
            Spinner coupleInput = FindViewById<Spinner>(Resource.Id.coupleInput);
            var couples = Repo.GetCouples().ToList();
            var couplesNames = couples.Select(x => x.CelebrityFirstName + " " + x.ProfessionalFirstName).ToList();
            couplesNames.Insert(0, "Select couple");
            coupleInput.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, couplesNames);
            try
            {
                coupleInput.ItemSelected += (sender, e) =>
                {
                    try
                    {
                        Spinner spinner = (Spinner)sender;
                        var selected = spinner.GetItemAtPosition(e.Position);
                        Score.CoupleID = couples.FirstOrDefault(x => selected.ToString().Contains(x.CelebrityFirstName) 
                        && selected.ToString().Contains(x.ProfessionalFirstName)).CoupleID;
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


        void ScoreInput()
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
                ScoreInput();
                if (Score.WeekNumber == 0 || Score.CoupleID == 0 || Score.DanceID == 0 || (Score.ScoreValue < 0 || Score.ScoreValue > 40))
                    Alert.ShowAlertWithSingleButton(this, "Error", "All fields must be populated", "OK");
                else
                {
                    Repo.SaveCoupleScore(Score);
                    Alert.ShowAlertWithSingleButton(this, "Sucess", "Successfully saved dance", "OK");
                    Scores = Repo.GetAllScores();
                }
               
            };
        }
    }
}