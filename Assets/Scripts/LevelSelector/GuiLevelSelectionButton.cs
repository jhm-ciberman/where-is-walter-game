

using System;
using UnityEngine;
using UnityEngine.UI;

public class GuiLevelSelectionButton : MonoBehaviour
{

    public GameLevel Level = GameLevel.Undefined;

    public GuiAvatar Avatar;

    public Button Button;

    public Color LockedColor = new Color(1f, 1f, 1f, 0.7f);

    public Color UnlockedColor = new Color(1f, 1f, 1f, 1f);

    public string LevelName
    {
        get
        {
            return this.Level.ToString().ToUpperInvariant();
        }
    }

    public void Start()
    {
        if (this.Level == GameLevel.Undefined)
        {
            throw new System.Exception($"LevelName is not set in {this.name}");
        }

        bool isUnlocked = LevelManager.Instance.IsLevelUnlocked(this.LevelName);
        this.SetUnlocked(isUnlocked);

        LevelManager.Instance.OnLevelUnlocked += this.OnLevelUnlocked;
    }

    public void OnDestroy()
    {
        LevelManager.Instance.OnLevelUnlocked -= this.OnLevelUnlocked;
    }

    private void OnLevelUnlocked(LevelInfo level)
    {
        if (level.Level == this.Level)
        {
            this.SetUnlocked(true);
        }
    }

    private void SetUnlocked(bool isUnlocked)
    {
        this.Button.interactable = isUnlocked;
        this.Avatar.SetColor(isUnlocked ? this.UnlockedColor : this.LockedColor);
    }

    public void OnClick()
    {
        if (!LevelManager.Instance.IsLevelUnlocked(this.LevelName))
        {
            return;
        }
        LevelManager.Instance.StartLevel(this.LevelName);
    }
}
