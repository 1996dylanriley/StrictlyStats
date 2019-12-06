using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Widget;
using Autofac;

namespace StrictlyStatistics
{
    [Activity(Label = "Instructions")]
    public class Instructions : Activity
    {
        public IRepository Repo { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Repo = MainApp.Container.Resolve<IRepository>();

            // Create your application here
            SetContentView(Resource.Layout.Instructions);
            var testView = FindViewById<TextView>(Resource.Id.textView2);
            try
            {
                testView.Text = String.Join('\n', Repo.GetAllDances().Select(x => x.Description).ToList());
            }
            catch (Exception e)
            {
                var x = e.Message;
            }
            
        }
    }
}