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

namespace StrictlyStatistics.UIComponents
{
    public static class WeekSpinner
    {
        public static void Initialise(StrictlyStatsActivity context, List<int> weeks, int spinnerId, bool addEventhandlers = true)
        {
            var weekInput = context.FindViewById<Spinner>(spinnerId);
            var adapter = new ArrayAdapter<int>(context, Android.Resource.Layout.SimpleListItem1, weeks);
            weekInput.Adapter = adapter;
            if (addEventhandlers)
            {
                weekInput.ItemSelected += (sender, e) =>
                {
                    Spinner spinner = (Spinner)sender;
                    var selected = spinner.GetItemAtPosition(e.Position);
                    string toast = string.Format("{0}", selected);
                    Toast.MakeText(context, toast, ToastLength.Long).Show();
                    context.SelectedWeek = (int)selected;
                };
            }

        }

        public static void Update(StrictlyStatsActivity context, List<int> weeks, int spinnerId) => Initialise(context, weeks, spinnerId, false);
    }

}