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
using StrictlyStatistics.Activities;
using StrictlyStatistics.Data.Models;

namespace StrictlyStatistics.UIComponents
{
    public static class CoupleSpinner
    {
        public static void Initialise(StrictlyStatsActivity context, List<Couple> couples, int spinnerId, bool addEventHandlers = true, Action ItemSelectedCallback = null)
        {
            var coupleInput = context.FindViewById<Spinner>(spinnerId);
            var spinnerItems = couples.Select(x => x.CoupleName).ToList();
            spinnerItems.Insert(0, "Select couple");
            coupleInput.Adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleListItem1, spinnerItems);
            if (addEventHandlers)
            {
                coupleInput.ItemSelected += (sender, e) =>
                {
                    Spinner spinner = (Spinner)sender;
                    var selected = spinner.GetItemAtPosition(e.Position);
                    context.Couple = couples.FirstOrDefault(x => selected.ToString().Contains(x.CoupleName)) ?? new Couple();
                    ItemSelectedCallback?.Invoke();
                };
            }     
        }

        public static void Update(StrictlyStatsActivity context, List<Couple> couples, int spinnerId) => Initialise(context, couples, spinnerId, false);
    }
}