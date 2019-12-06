using System;
using Android.App;
using Android.Runtime;
using Autofac;

namespace StrictlyStatistics
{
    [Application]
    public class MainApp : Application
    {
        public static IContainer Container { get; set; }

        public MainApp(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<SQLiteRepository>().As<IRepository>().SingleInstance();
                Container = builder.Build();
            }
            catch (Exception e)
            {
                var x = e.Message;
            }
            

        }
    }
}