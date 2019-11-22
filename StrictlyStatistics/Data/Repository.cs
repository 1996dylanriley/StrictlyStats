
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace StrictlyStatistics
{
    public interface IRepository
    {

    }

    public class SQLiteRepository : IRepository
    {
        SQLiteConnection con;

        public string DbLocation
        {
            get
            {
                string fileName = "StrictlyStats.db";
                string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var path = Path.Combine(libraryPath, fileName);
                return path;
            }
        }

        public SQLiteRepository()
        {
            con = new SQLiteConnection(DbLocation);
        }

        public List<Dance> GetAllDances()
        {
            List<Dance> dances = con.Table<Dance>().ToList();

            return dances;
        }
    }
}