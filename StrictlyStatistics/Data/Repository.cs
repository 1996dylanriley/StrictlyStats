
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
        List<Couple> GetAllCouples();
        Couple GetCouple(int id);
        void SaveCoupleScore(Score score);
        void UpdateScore(Score score);
        void UpdateCouple(Couple couple);
        void UpdateDance(Dance dance);
        void CreateCouple(Dance couple);
        void RemoveCouple(Dance couple);
        void RemoveScore(Score score);
        void CreateDance(Dance dance);
        void RemoveDance(Dance dance);


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

        //Dances
        public List<Dance> GetAllDances() => con.Table<Dance>().ToList();
        public Dance GetDance(int id) => con.Table<Dance>().FirstOrDefault(x => x.DanceId == id);
        public void UpdateDance(Dance dance) => con.Update(dance);
        public void CreateDance(Dance dance) => con.Insert(dance);
        public void RemoveDance(Dance dance) => con.Delete(dance);

        //Scores
        public List<Score> GetAllScores() => con.Table<Score>().ToList();
        public Score GetScore(int id) => con.Table<Score>().FirstOrDefault(x => x.ScoreID == id);
        public void SaveCoupleScore(Score score) => con.Insert(score);
        public void UpdateScore(Score score) => con.Update(score);
        public void RemoveScore(Score score) => con.Delete(score);

        //Couples
        public List<Couple> GetAllCouples() => con.Table<Couple>().ToList();
        public Couple GetCouple(int id) => con.Table<Couple>().FirstOrDefault(x => x.CoupleID == id);
        public void UpdateCouple(Couple couple) => con.Update(couple);
        public void CreateCouple(Dance couple) => con.Insert(couple);
        public void RemoveCouple(Dance couple) => con.Delete(couple);
    }
}