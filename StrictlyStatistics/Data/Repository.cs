
using SQLite;
using StrictlyStatistics.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StrictlyStatistics
{
    public interface IRepository
    {
        List<Dance> GetAllDances();
        List<Score> GetAllScores();
        List<Score> AddScore();
        List<Couple> GetCouples();
        void SaveCoupleScore(Score score);
        void OverwrtireScore(Score score);

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
            try
            {
                con = new SQLiteConnection(DbLocation);
            }
             
            catch(Exception e)
            {
                var x = e.Message;
            }
        }

        public List<Dance> GetAllDances()
        {
            List<Dance> dances = con.Table<Dance>().ToList();

            return dances;
        }

        public List<Score> GetAllScores()
        {
            List<Score> scores = con.Table<Score>().ToList();

            return scores;
        }

        public List<Score> AddScore()
        {
            List<Score> scores = con.Table<Score>().ToList();

            return scores;
        }

        public List<Couple> GetCouples()
        {
            List<Couple> couples = con.Table<Couple>().ToList();

            return couples;
        }

        public void SaveCoupleScore(Score score)
        {
            try
            {
                con.Insert(score);
            }
            catch(Exception e)
            {
                var x = e.Message;
            }
        }

        public void OverwrtireScore(Score score)
        {
            var scoresToDelete = GetAllScores().Where(x => x.CoupleID == score.CoupleID && x.WeekNumber == score.WeekNumber);
            foreach (var s in scoresToDelete)
                con.Delete(s);

            SaveCoupleScore(score);
        }
    }
}