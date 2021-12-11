using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCDiscordScorebot
{
    class Score
    {
        public string levelId, songName, levelAuthor, songAuthor, difficulty;
        public int userId, difficultyRank, difficultyRaw, score, modifiedScore, goodCuts, badCuts, missed, notGood, ok, averageCutScore, maxCutScore, maxCombo;
        public bool fullCombo;
        public float leftSaberDistance, leftHandDistance, rightSaberDistance, rightHandDistance, averageCutDistanceRawScore, minDirDeviation, maxDirDeviation, averageDirDeviation, minTimeDeviation, maxTimeDeviation, averageTimeDeviation;
    }
}
