using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public event Action<LevelInfo> OnLevelUnlocked;

    public static LevelManager Instance { get; private set; }

    private readonly List<LevelInfo> _levels = new List<LevelInfo>()
    {
        new LevelInfo(GameLevel.Cuarto).Unlocks(GameLevel.Disco),
        new LevelInfo(GameLevel.Disco).Unlocks(GameLevel.BigBen),
        new LevelInfo(GameLevel.BigBen).Unlocks(GameLevel.Bar),
        new LevelInfo(GameLevel.Bar),
    };

    private readonly Dictionary<string, LevelInfo> _levelsByName = new Dictionary<string, LevelInfo>();

    private readonly List<LevelInfo> _unlockedLevels = new List<LevelInfo>();

    private readonly List<LevelInfo> _recentlyUnlockedLevels = new List<LevelInfo>();

    public int UnlockedLevelsCount => this._unlockedLevels.Count;

    public void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this.gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(this.gameObject);

        foreach (var levelInfo in this._levels)
        {
            this._levelsByName.Add(levelInfo.SceneName, levelInfo);
        }

        this.UnlockLevel(this._levels[0].SceneName);
    }

    public void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.U))
        {
            LevelInfo lockedLevel = this._levels.Where(l => !this._unlockedLevels.Contains(l)).FirstOrDefault();

            if (lockedLevel != null)
            {
                this.UnlockLevel(lockedLevel.SceneName);
            }
        }
    }

    private LevelInfo GetLevelInfo(string levelName)
    {
        if (this._levelsByName.TryGetValue(levelName, out var level))
        {
            return level;
        }

        throw new System.Exception($"Level {levelName} not found");
    }

    public void UnlockLevel(string levelName)
    {
        var level = this.GetLevelInfo(levelName);
        if (!this._unlockedLevels.Contains(level))
        {
            this._unlockedLevels.Add(level);
            this._recentlyUnlockedLevels.Add(level);
            this.OnLevelUnlocked?.Invoke(level);
        }
    }

    public bool IsLevelUnlocked(string levelName)
    {
        var level = this.GetLevelInfo(levelName);
        return this._unlockedLevels.Contains(level);
    }

    public bool IsLevelRecentlyUnlocked(string levelName)
    {
        var level = this.GetLevelInfo(levelName);
        return this._recentlyUnlockedLevels.Contains(level);
    }

    public void ResetRecentlyUnlockedLevels()
    {
        this._recentlyUnlockedLevels.Clear();
    }

    public void NotifyLevelCompleted(string levelName = null)
    {
        levelName ??= SceneManager.GetActiveScene().name;

        var level = this.GetLevelInfo(levelName);
        foreach (var nextLevel in level.UnlocksLevels)
        {
            Debug.Log("Unlocking level: " + nextLevel.ToString().ToUpperInvariant());
            this.UnlockLevel(nextLevel.ToString().ToUpperInvariant());
        }
    }
}
