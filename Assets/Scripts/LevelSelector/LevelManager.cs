using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager
{
    public event Action<GameLevel> OnLevelUnlocked;

    private static LevelManager _instance;

    public static LevelManager Instance => _instance ??= new LevelManager();

    private readonly List<GameLevel> _levels = new List<GameLevel>()
    {
        GameLevel.Cuarto,
        GameLevel.Disco,
        GameLevel.BigBen,
        GameLevel.Bar,
    };

    public LevelManager()
    {
        this.UnlockLevel(this._levels.First());
    }

    public void UnlockLevel(GameLevel level)
    {
        if (!this.IsLevelUnlocked(level))
        {
            PlayerPrefs.SetInt(level.ToString() + "_unlocked", 1);
            PlayerPrefs.Save();

            Debug.Log($"Level {level} unlocked");
            this.OnLevelUnlocked?.Invoke(level);
        }

    }

    public bool IsLevelUnlocked(GameLevel level)
    {
        return PlayerPrefs.GetInt(level.ToString() + "_unlocked", 0) == 1;
    }

    public void NotifyLevelCompleted(GameLevel level = GameLevel.Undefined)
    {
        level = level == GameLevel.Undefined ? this.FindLevelBySceneName(SceneManager.GetActiveScene().name) : level;

        int index = this._levels.IndexOf(level);

        if (index == -1)
        {
            throw new Exception($"Level {level} not found in level list");
        }

        if (index < this._levels.Count - 1)
        {
            GameLevel nextLevel = this._levels[index + 1];

            if (!this.IsLevelUnlocked(nextLevel))
            {
                this.UnlockLevel(nextLevel);
                this.OnLevelUnlocked?.Invoke(nextLevel);
            }
        }
    }

    private static string GetSceneName(GameLevel level)
    {
        return level.ToString().ToUpperInvariant();
    }

    public GameLevel FindLevelBySceneName(string sceneName)
    {
        sceneName = sceneName.ToUpperInvariant();
        return this._levels.FirstOrDefault(l => GetSceneName(l) == sceneName);
    }

    public GameLevel GetFirstUnlockedLevel()
    {
        return this._levels.FirstOrDefault(l => this.IsLevelUnlocked(l));
    }

    public void LoadNextLevel()
    {
        var levelToPlay = this.GetFirstUnlockedLevel();
        SceneManager.LoadScene(GetSceneName(levelToPlay));
    }
}
