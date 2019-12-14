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

namespace StrictlyStatistics.Activities
{
    public class StrictlyStatsActivity : Activity
    {
        public IRepository Repo { get; set; }
        internal Score Score { get; set; }
        internal Couple Couple { get; set; }
        internal int SelectedWeek { get; set; }
        internal List<int> weeks = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Repo = MainApp.Container.Resolve<IRepository>();
        }
    }
}