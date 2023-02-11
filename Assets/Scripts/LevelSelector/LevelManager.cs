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

    public enum LevelStatus
    {
        Unplayed = 0,
        Played = 1,
    }

    private readonly Dictionary<GameLevel, LevelStatus> _levelStatus = new();

    private readonly Dictionary<GameLevel, AvatarAppearance[]> _playerLevelAvatars = new();

    public LevelManager()
    {
        bool isInEditor = Application.isEditor;

        // Reset all levels
        if (isInEditor)
        {
            foreach (GameLevel level in this._levels)
            {
                this.SetLevelStatus(level, LevelStatus.Unplayed);
            }
        }
    }

    private void SetLevelStatus(GameLevel level, LevelStatus status)
    {
        Debug.Log($"Setting level {level} status to {status}");
        this._levelStatus[level] = status;
    }

    public LevelStatus GetLevelStatus(GameLevel level)
    {
        return this._levelStatus.TryGetValue(level, out LevelStatus status) ? status : LevelStatus.Unplayed;
    }

    public bool IsLevelWinned(GameLevel level)
    {
        return this.GetLevelStatus(level) == LevelStatus.Played;
    }

    public GameLevel GetCurrentLevel()
    {
        return this.FindLevelBySceneName(SceneManager.GetActiveScene().name);
    }

    public void NotifyLevelCompleted(GameLevel level, IEnumerable<AvatarAppearance> avatars = null)
    {
        this.SetLevelStatus(level, LevelStatus.Played);
    }

    public void SaveAvatarsForLevel(GameLevel level, IEnumerable<AvatarAppearance> avatars)
    {
        this._playerLevelAvatars[level] = avatars.ToArray();
    }

    public void UnlockLevel(GameLevel level)
    {
        this.SetLevelStatus(level, LevelStatus.Unplayed);
        this.OnLevelUnlocked?.Invoke(level);
    }

    private static string GetSceneName(GameLevel level)
    {
        return level.ToString().ToUpperInvariant();
    }

    public GameLevel FindLevelBySceneName(string sceneName)
    {
        sceneName = sceneName.ToUpperInvariant();
        var level = this._levels.FirstOrDefault(l => GetSceneName(l) == sceneName);

        if (level == GameLevel.Undefined)
        {
            throw new Exception($"Cannot find level for scene {sceneName}");
        }

        return level;
    }

    public GameLevel GetNextLevelToPlay()
    {
        return this._levels.FirstOrDefault(l => this.GetLevelStatus(l) == LevelStatus.Unplayed);
    }

    public void LoadNextLevel()
    {
        GameLevel nextLevel = this.GetNextLevelToPlay();

        if (nextLevel == GameLevel.Undefined)
        {
            Debug.Log("No more levels to unlock");
            return;
        }

        SceneManager.LoadScene(GetSceneName(nextLevel));
    }

    public AvatarAppearance GetAvatarForLevel(GameLevel level, int index)
    {
        if (!this._playerLevelAvatars.TryGetValue(level, out AvatarAppearance[] avatars))
        {
            throw new Exception($"Cannot find avatars for level {level}");
        }

        if (index < 0 || index >= avatars.Length)
        {
            throw new Exception($"Cannot find avatar {index} for level {level}. Only {avatars.Length} avatars found");
        }

        return avatars[index];
    }

    public bool HasSavedAvatarsForLevel(GameLevel level)
    {
        return this._playerLevelAvatars.ContainsKey(level);
    }
}
