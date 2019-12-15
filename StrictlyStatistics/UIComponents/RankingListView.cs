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
using StrictlyStatistics.Adapters;

namespace StrictlyStatistics.UIComponents
{
    public static class RankingListView
    {
        public static void Initialise(Activity context, List<Tuple<string, int>> items, int listId)
        {
            var listView = context.FindViewById<ListView>(listId);

            items.Sort((x, y) => y.Item2.CompareTo(x.Item2));

            for (int i = 0; i < items.Count; i++)
            {
                int rankNumber = i + 1;
                items[i] = new Tuple<string, int>("#" + rankNumber.ToString() + " " + items[i].Item1, items[i].Item2);
            }

            var adapter = new SimpleListItem2ListAdapter(context, items);
            listView.Adapter = adapter;
        }
    }
}