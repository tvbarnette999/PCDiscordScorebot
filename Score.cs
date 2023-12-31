using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCDiscordScorebot
{
    class Score
    {
        public string userId, levelId, songName, levelAuthor, songAuthor, difficulty, modifiers;
        public int difficultyRank, difficultyRaw, score, modifiedScore, multipliedScore, goodCuts, badCuts, missed, notGood, ok, averageCutScore, maxCutScore, maxCombo;
        public bool fullCombo, withoutMods;
        public float leftSaberDistance, leftHandDistance, rightSaberDistance, rightHandDistance, averageCutDistanceRawScore, minDirDeviation, maxDirDeviation, averageDirDeviation, minTimeDeviation, maxTimeDeviation, averageTimeDeviation;
    }
}
