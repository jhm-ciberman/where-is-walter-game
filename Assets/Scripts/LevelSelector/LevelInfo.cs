using System.Collections.Generic;

[System.Serializable]
public class LevelInfo
{
    public GameLevel Level;

    public string SceneName => this.Level.ToString().ToUpperInvariant();

    public List<GameLevel> UnlocksLevels = new List<GameLevel>();


    public LevelInfo(GameLevel level)
    {
        this.Level = level;
    }

    public LevelInfo Unlocks(GameLevel level)
    {
        this.UnlocksLevels.Add(level);
        return this;
    }
}
