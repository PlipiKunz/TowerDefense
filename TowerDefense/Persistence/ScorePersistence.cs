using System;
using System.Collections.Generic;
using System.Text;

namespace CS5410.Persistence
{
    static class ScorePersistence
    {
        private static PersistScore p = new PersistScore();


        public static List<int> scores = new List<int>();
        public static int score = 0;

        public static void addScore(int score) {
            ScorePersistence.score = score;

            scores.Add(score);
            scores.Sort();
            scores.Reverse();

            while (scores.Count > 5) {
                scores.RemoveAt(scores.Count-1);
            }
        }


        public static Boolean loaded = false;
        public static void getPersistedScores()
        {
            p.loadScores();
            while (!loaded)
            {
                if (PersistScore.m_loadedScores != null)
                {
                    scores = PersistScore.m_loadedScores;
                    loaded = true;
                }
                else if (PersistScore.m_loadedScores == null && PersistScore.scoresExists == false)
                {
                    loaded = true;
                }
            }
        }

        public static void persistScores()
        {

            p.saveScores();
        }
    }
}
