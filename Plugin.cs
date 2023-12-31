using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BS_Utils.Utilities;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using IPALogger = IPA.Logging.Logger;
using PCDiscordScorebot.Configuration;

namespace PCDiscordScorebot
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger, IPA.Config.Config conf)
        {
            Instance = this;
            Log = logger;
            PluginConfig.Instance = conf.Generated<PluginConfig>();
            Log.Info("Discord Scorebot config: " + PluginConfig.Instance.submitData.userId + "@" + PluginConfig.Instance.submitData.url);
            Log.Info("PCDiscordScorebot initialized.");
        }

        #region BSIPA Config
        //Uncomment to use BSIPA's config
        /*
        [Init]
        public void InitWithConfig(Config conf)
        {
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded");
        }
        */
        #endregion

        [OnStart]
        public void OnApplicationStart()
        {
            Log.Debug("OnApplicationStart");

            BSEvents.levelCleared += submitScore;

            new GameObject("PCDiscordScorebotController").AddComponent<PCDiscordScorebotController>();

        }

        private string difficultyToString(BeatmapDifficulty difficulty)
        {
            switch (difficulty)
            {
                case BeatmapDifficulty.Easy:
                    return "Easy";
                case BeatmapDifficulty.Normal:
                    return "Normal";
                case BeatmapDifficulty.Hard:
                    return "Hard";
                case BeatmapDifficulty.Expert:
                    return "Expert";
                case BeatmapDifficulty.ExpertPlus:
                    return "Expert+";
            }
            return "Unknown";
        }

        private void submitScore(StandardLevelScenesTransitionSetupDataSO data, LevelCompletionResults results) {
            if (data.practiceSettings != null)
            {
                Log.Info("Level Completed in practice.");
                return;
            }
            Score score = new Score();
            score.userId = PluginConfig.Instance.submitData.userId;
            score.levelId = data.difficultyBeatmap.level.levelID;
            score.songName = data.difficultyBeatmap.level.songName;
            score.levelAuthor = data.difficultyBeatmap.level.levelAuthorName;
            score.songAuthor = data.difficultyBeatmap.level.songAuthorName;
            score.difficulty = difficultyToString(data.difficultyBeatmap.difficulty);
            score.difficultyRank = data.difficultyBeatmap.difficultyRank;
            score.difficultyRaw = (int) data.difficultyBeatmap.difficulty;
            score.score = results.totalCutScore;
            score.multipliedScore = results.multipliedScore;
            score.withoutMods = results.gameplayModifiers.IsWithoutModifiers();
            score.modifiers = results.gameplayModifiers.ToString();
            score.modifiedScore = results.modifiedScore;
            score.fullCombo = results.fullCombo;
            score.goodCuts = results.goodCutsCount;
            score.badCuts = results.badCutsCount;
            score.missed = results.missedCount;
            score.notGood = results.notGoodCount;
            score.maxCombo = results.maxCombo;
            score.leftHandDistance = results.leftHandMovementDistance;
            score.rightHandDistance = results.rightHandMovementDistance;
            score.leftSaberDistance = results.leftSaberMovementDistance;
            score.rightSaberDistance = results.rightHandMovementDistance;
            score.maxCutScore = results.maxCutScore;           
            score.averageCutScore = 0;
            score.minDirDeviation = 0;
            score.averageDirDeviation = 0;
            score.maxDirDeviation = 0;
            score.minTimeDeviation = 0;
            score.averageTimeDeviation = 0;
            score.maxTimeDeviation = 0;
            score.ok = results.okCount;
            string json = JsonConvert.SerializeObject(score, Formatting.None);
            AsyncOperation op = UnityWebRequest.Post(PluginConfig.Instance.submitData.url, json).Send();
            Log.Info("Level Complete: " +  json);

        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");

        }
    }
}
