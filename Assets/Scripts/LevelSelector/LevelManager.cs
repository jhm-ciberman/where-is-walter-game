using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public event Action<LevelInfo> OnLevelUnlocked;

    public static LevelManager Instance { get; private set; }

    public List<LevelInfo> Levels;

    private readonly Dictionary<string, LevelInfo> _levelsByName = new Dictionary<string, LevelInfo>();

    public string LevelSelectionSceneName = "MenuArbol";

    private readonly List<LevelInfo> _unlockedLevels = new List<LevelInfo>();

    private readonly List<LevelInfo> _recentlyUnlockedLevels = new List<LevelInfo>();

    public LevelInfo CurrentLevel { get; private set; } = null;

    public int UnlockedLevelsCount => this._unlockedLevels.Count;

    public void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        foreach (var levelInfo in this.Levels)
        {
            this._levelsByName.Add(levelInfo.SceneName, levelInfo);
        }

        this.UnlockLevel(this.Levels[0].SceneName);
    }

    public void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.U))
        {
            LevelInfo lockedLevel = this.Levels.Where(l => !this._unlockedLevels.Contains(l)).FirstOrDefault();

            if (lockedLevel != null)
            {
                this.UnlockLevel(lockedLevel.SceneName);
            }
        }
    }

    public void StartLevel(string levelName)
    {
        if (this.IsLevelUnlocked(levelName))
        {
            var level = this.GetLevelInfo(levelName);
            this.CurrentLevel = level;
            SceneManager.LoadScene(level.SceneName);
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
        levelName ??= this.CurrentLevel.SceneName;

        var level = this.GetLevelInfo(levelName);
        foreach (var nextLevel in level.UnlocksLevels)
        {
            this.UnlockLevel(nextLevel.ToString().ToUpperInvariant());
        }
    }
}
