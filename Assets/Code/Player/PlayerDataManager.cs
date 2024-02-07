using System.Collections.Generic;
using System.Linq;
using Code.Utils;
using UnityEngine;

namespace Code.Player
{
    [System.Serializable]
    public class PlayerLevelData
    {
        public int levelNumberIndex;
        public int score;
        public bool isCompleted;
        public bool isActiveLevel;

        public PlayerLevelData(int levelNumberIndex, int score, bool isCompleted, bool isActiveLevel)
        {
            this.levelNumberIndex = levelNumberIndex;
            this.score = score;
            this.isCompleted = isCompleted;
            this.isActiveLevel = isActiveLevel;
        }
    }

    [System.Serializable]
    public class PlayerLevelDataList
    {
        public List<PlayerLevelData> levelDataList = new List<PlayerLevelData>();
    }

    public class PlayerDataManager : GenericSingleton<PlayerDataManager>
    {
        private const string LevelDataKey = "PlayerLevelData";
        private int _activeLevelIndex = -1; // Cached active level index

        public int ActiveLevelIndex => _activeLevelIndex;

        protected override void Awake()
        {
            base.Awake();
            InitializePlayerData(); 
        }
        
        private void InitializePlayerData()
        {
            if (!PlayerPrefs.HasKey(LevelDataKey))
            {
                var initialLevelData = new List<PlayerLevelData>
                {
                    new PlayerLevelData(0, 0, false, true)
                };
                SetAllLevelData(initialLevelData);
                _activeLevelIndex = 0;
            }
            else
            {
                UpdateCachedActiveLevel(); // Update the cached active level based on existing data
            }
        }


        public List<PlayerLevelData> GetAllLevelData()
        {
            string jsonString = PlayerPrefs.GetString(LevelDataKey, "");
            Debug.LogError("PlayerPrefs Json : "+jsonString);
            if (string.IsNullOrEmpty(jsonString)) return new List<PlayerLevelData>();

            PlayerLevelDataList levelDataList = JsonUtility.FromJson<PlayerLevelDataList>(jsonString);
            Debug.LogError("PlayerLevelDataList count : "+levelDataList);
            return levelDataList?.levelDataList ?? new List<PlayerLevelData>();
        }

        public void SetAllLevelData(List<PlayerLevelData> levelDataList)
        {
            PlayerLevelDataList wrapper = new PlayerLevelDataList { levelDataList = levelDataList };
            string jsonString = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString(LevelDataKey, jsonString);
            PlayerPrefs.Save();
            UpdateCachedActiveLevel(); 
        }

        public void SavePlayerResult(PlayerLevelData newResult)
        {
            List<PlayerLevelData> levelDataList = GetAllLevelData();
            int index = levelDataList.FindIndex(level => level.levelNumberIndex == newResult.levelNumberIndex);
            if (index != -1)
            {
                levelDataList[index] = newResult;
            }
            else
            {
                levelDataList.Add(newResult); // Add new result if not found
            }
            SetAllLevelData(levelDataList);
            if (newResult.isActiveLevel)
            {
                _activeLevelIndex = newResult.levelNumberIndex; // Update cached active level if this result is for the active level
            }
            UpdateCachedActiveLevel();
        }


        private void UpdateCachedActiveLevel()
        {
            List<PlayerLevelData> levelDataList = GetAllLevelData();
            PlayerLevelData activeLevel = levelDataList.Find(level => level.isActiveLevel);

            if (activeLevel != null)
            {
                // If there's an active level, use its index
                _activeLevelIndex = activeLevel.levelNumberIndex;
            }
            else if (levelDataList.Count > 0)
            {
                // If there's no active level, but there are completed levels, use the next index
                int highestLevelIndex = levelDataList.Max(level => level.levelNumberIndex);
                _activeLevelIndex = highestLevelIndex + 1;
            }
            else
            {
                _activeLevelIndex = -1;
            }
        }

        public void ClearPlayerData()
        {
            PlayerPrefs.DeleteKey(LevelDataKey);
            PlayerPrefs.Save();
            _activeLevelIndex = -1; // Reset cached active level index when data is cleared
        }
    }
}