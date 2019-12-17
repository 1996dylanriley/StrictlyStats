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
    [Activity(Label = "EditCouple")]
    public class EditCouple : StrictlyStatsActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditCouple);

            InitialiseInputs(true);
            CoupleSpinner.Initialise(this, Repo.GetAllCouples(), Resource.Id.editCoupleSpinner, true, PopulateInputs);
        }

        public void InitialiseInputs(bool addEventhandlers)
        {
            var celebFirstNameInput = FindViewById<EditText>(Resource.Id.celebFirstNameInput);
            celebFirstNameInput.Text = Couple?.CelebrityFirstName;

            var celebLastNameInput = FindViewById<EditText>(Resource.Id.celebLastNameInput);
            celebLastNameInput.Text = Couple?.CelebrityLastName;

            var proFirstNameInput = FindViewById<EditText>(Resource.Id.proFirstNameInput);
            proFirstNameInput.Text = Couple?.ProfessionalFirstName;

            var proLastNameInput = FindViewById<EditText>(Resource.Id.proLastNameInput);
            proLastNameInput.Text = Couple?.ProfessionalLastName;

            var celebStarRatingInput = FindViewById<EditText>(Resource.Id.celebStarRating);
            celebStarRatingInput.Text = Couple?.CelebrityStarRating.ToString();

            var votedOffWeekNumberInput = FindViewById<EditText>(Resource.Id.votedOffWeekNumber);
            votedOffWeekNumberInput.Text = Couple?.VotedOffWeekNumber.ToString();

            var saveButton = FindViewById<Button>(Resource.Id.editCoupleSaveButton);
            var cancelButton = FindViewById<Button>(Resource.Id.editCoupleCancelButton);

            if (addEventhandlers)
            {
                celebFirstNameInput.TextChanged += (sender, args) => Couple.CelebrityFirstName = args.Text?.ToString() ?? "";
                celebLastNameInput.TextChanged += (sender, args) => Couple.CelebrityLastName = args.Text?.ToString() ?? "";
                proFirstNameInput.TextChanged += (sender, args) => Couple.ProfessionalFirstName = args.Text?.ToString() ?? "";
                proLastNameInput.TextChanged += (sender, args) => Couple.ProfessionalLastName = args.Text?.ToString() ?? "";

                celebStarRatingInput.TextChanged += (sender, args) =>
                {                    
                    int starRating = 0;
                    int.TryParse(args.Text.ToString(), out starRating);
                    if (starRating != 0)
                        Couple.CelebrityStarRating = starRating;
                };

                votedOffWeekNumberInput.TextChanged += (sender, args) => 
                {
                    int votedOff = 0;
                    int.TryParse(args.Text.ToString(), out votedOff);
                    if (votedOff != 0)
                        Couple.VotedOffWeekNumber = votedOff;
                };

                saveButton.Click += (sender, args) =>
                {
                    if (Couple.CoupleID != 0) Repo.UpdateCouple(Couple);
                };

                cancelButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(EditCouple)));
            }
        }

        public void PopulateInputs() => InitialiseInputs(false);


    }
}