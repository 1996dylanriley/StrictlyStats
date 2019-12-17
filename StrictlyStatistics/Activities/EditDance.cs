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
using StrictlyStatistics.UIComponents;

namespace StrictlyStatistics.Activities
{
    [Activity(Label = "EditDance")]
    public class EditDance : StrictlyStatsActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditDance);

            InitialiseInputs(true);
            DanceSpinner.Initialise(this, Repo.GetAllDances(), Resource.Id.editDanceSpinner, true, PopulateInputs);
        }

        public void InitialiseInputs(bool addEventhandlers)
        {
            var editDanceNameInput = FindViewById<EditText>(Resource.Id.editDanceNameInput);
            editDanceNameInput.Text = Dance?.Name;

            var editDanceDescriptionInput = FindViewById<EditText>(Resource.Id.editDanceDescriptionInput);
            editDanceDescriptionInput.Text = Dance?.Description;

            var editDanceDifficultyInput = FindViewById<EditText>(Resource.Id.editDanceDifficultyInput);
            editDanceDifficultyInput.Text = Dance?.DegreeOfDifficulty.ToString();

            var saveButton = FindViewById<Button>(Resource.Id.editDanceSaveButton);
            var cancelButton = FindViewById<Button>(Resource.Id.editDanceCancelButton);
            var deleteButton = FindViewById<Button>(Resource.Id.deleteDanceButton);

            if (addEventhandlers)
            {
                editDanceNameInput.TextChanged += (sender, args) => Dance.Name = args.Text?.ToString() ?? "";
                editDanceDescriptionInput.TextChanged += (sender, args) => Dance.Description = args.Text?.ToString() ?? "";

                editDanceDifficultyInput.TextChanged += (sender, args) =>
                {
                    int diif = 0;
                    int.TryParse(args.Text.ToString(), out diif);
                    if (diif != 0)
                        Dance.DegreeOfDifficulty = diif;
                };

                saveButton.Click += (sender, args) =>
                {
                    if (Dance.DegreeOfDifficulty == 0 || String.IsNullOrEmpty(Dance.Description) || String.IsNullOrEmpty(Dance.Name))
                        Alert.ShowAlertWithSingleButton(this, "Error", "All fields must be populated", "OK");
                    else if (Dance.DanceId != 0)
                        Repo.UpdateDance(Dance);
                    else
                    {
                        Repo.CreateDance(Dance);
                        Alert.ShowAlertWithSingleButton(this, "Success", "Dance created", "OK");
                    }
                        
                };

                cancelButton.Click += (sender, args) => StartActivity(new Intent(this, typeof(EditDance)));

                deleteButton.Click += (sender, args) =>
                {
                    if(Dance.DanceId != 0)
                    {
                        Repo.RemoveDance(Dance);
                        Alert.ShowAlertWithSingleButton(this, "Success", "Dance deleted", "OK");
                    }
                };
            }
        }

        public void PopulateInputs() => InitialiseInputs(false);
    }
}