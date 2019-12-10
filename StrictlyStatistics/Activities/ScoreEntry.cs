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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Repo = MainApp.Container.Resolve<IRepository>();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ScoreEntry);
            InitialiseComponents();
        }

        public void InitialiseComponents()
        {
            InitialiseWeekInput();
            InitialiseDanceInput();
            InitialiseConfirmButton();
            CreateHandlers();
        }
        


        void InitialiseWeekInput()
        {
            int[] weeks = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

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
                        Score.DanceID = (int)selected;
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
            var dances = Repo.GetAllDances().Select(x => x.Name).ToArray();
            danceInput.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, dances);
            try
            {
                danceInput.ItemSelected += (sender, args) =>
                {
                    try
                    {
                        Score.DanceID = (int)danceInput.SelectedItemId;
                    }
                    catch (Exception e)
                    {
                        var x = e.Message;
                    }
                };
            }
            catch(Exception e)
            {
                var x = e.Message;
            }
        }

        void InitialiseScoreInput()
        {
            NumberPicker scoreInput = FindViewById<NumberPicker>(Resource.Id.scoreInput);
            scoreInput.MinValue = 0;
            scoreInput.MaxValue = 40;
            
            try
            {
                scoreInput.ValueChanged += (sender, args) => Score.ScoreValue = scoreInput.Value;
            }
            catch (Exception e)
            {
                var x = e.Message;
            }

        }

        void InitialiseConfirmButton()
        {
            Button button = FindViewById<Button>(Resource.Id.confirmButton);

            button.Click += (sender, args) =>
            {
                if (Score.CoupleID == 0 || Score.DanceID == 0 || Score.ScoreValue < 0)
                    Alert.ShowAlertWithSingleButton(this, "Error", "All fields must be populated", "OK");
                else { }
                    //temp
            };
        }



        void CreateHandlers()
        {
            //var instructionsButton = FindViewById<Button>(Resource.Id.InstructionsButton);
            //instructionsButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(Instructions));

            //WeekSpinner.ItemSelected += (sender, args) => Toast.MakeText(this, weeks[0], ToastLength.Short).Show(); 
        }
    }
}