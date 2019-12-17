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
    public static class DanceSpinner
    {
        public static void Initialise(StrictlyStatsActivity context, List<Dance> dances, int spinnerId, bool addEventhandlers = true, Action itemSelectedCallback = null)
        {
            var danceInput = context.FindViewById<Spinner>(spinnerId);
            var danceNames = dances.Select(x => x.Name).ToList();
            danceNames.Insert(0, "Select dance");
            danceInput.Adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleListItem1, danceNames);

            if (addEventhandlers)
            {
                danceInput.ItemSelected += (sender, e) =>
                {
                    Spinner spinner = (Spinner)sender;
                    var selected = spinner.GetItemAtPosition(e.Position);
                    context.Dance = dances.FirstOrDefault(x => x.Name == selected.ToString()) ?? new Dance();
                    itemSelectedCallback?.Invoke();
                };
            }
        }

        public static void Update(StrictlyStatsActivity context, List<Dance> dances, int spinnerId) => Initialise(context, dances, spinnerId, false);
    }
}