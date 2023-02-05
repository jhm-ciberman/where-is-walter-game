[System.Serializable]
public class LevelInfo
{
    public GameLevel Level;

    public string SceneName => this.Level.ToString().ToUpperInvariant();

    public GameLevel[] UnlocksLevels;
}
