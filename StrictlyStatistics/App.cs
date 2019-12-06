using System;
using System.IO;
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
            Sqlite();

            var builder = new ContainerBuilder();
            builder.RegisterType<SQLiteRepository>().As<IRepository>().SingleInstance();
            Container = builder.Build();

        }

        public void Sqlite()
        {
            var docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var dbFile = Path.Combine(docFolder, "StrictlyStats.db");
            if (!System.IO.File.Exists(dbFile))
            {
                var stream = Resources.OpenRawResource(Resource.Raw.StrictlyStats);
                FileStream writeStream = new FileStream(dbFile, FileMode.OpenOrCreate, FileAccess.Write);
                ReadWriteStream(stream, writeStream);
            }
        }

        private void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int length = 256;
            byte[] buffer = new byte[length];
            int bytesRead = readStream.Read(buffer, 0, length);

            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, length);
            }

            readStream.Close();
            writeStream.Close();
        }
    }
}