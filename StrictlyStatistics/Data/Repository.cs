
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
        Dance GetDance(int id);
        List<Score> GetAllScores();
        Score GetScore(int id);
        List<Score> AddScore();
        List<Couple> GetCouples();
        Couple GetCouple(int id);
        void SaveCoupleScore(Score score);
        void OverwrtireScore(Score score);
        void UpdateCouple(Couple couple);

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

        public SQLiteRepository() => con = new SQLiteConnection(DbLocation);

        public List<Dance> GetAllDances() => con.Table<Dance>().ToList();
        public Dance GetDance(int id) => con.Table<Dance>().FirstOrDefault(x => x.DanceId == id);

        public List<Score> GetAllScores() => con.Table<Score>().ToList();
        public Score GetScore(int id) => con.Table<Score>().FirstOrDefault(x => x.ScoreID == id);

        public List<Score> AddScore() => con.Table<Score>().ToList();

        public List<Couple> GetCouples() => con.Table<Couple>().ToList();

        public Couple GetCouple(int id) => con.Table<Couple>().FirstOrDefault(x => x.CoupleID == id);

        public void SaveCoupleScore(Score score) => con.Insert(score);

        public void OverwrtireScore(Score score)
        {
            var scoresToDelete = GetAllScores().Where(x => x.CoupleID == score.CoupleID && x.WeekNumber == score.WeekNumber);
            foreach (var s in scoresToDelete)
                con.Delete(s);

            SaveCoupleScore(score);
        }

        public void UpdateCouple(Couple couple) => con.Update(couple);
    }
}