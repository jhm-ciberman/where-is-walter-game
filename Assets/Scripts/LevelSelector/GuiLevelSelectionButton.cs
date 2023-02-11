

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuiLevelSelectionButton : MonoBehaviour
{

    public GameLevel Level = GameLevel.Undefined;

    public int AvatarIndex = -1;

    public GuiAvatar Avatar;

    public Button Button;

    public Color LockedColor = new Color(1f, 1f, 1f, 0.7f);

    public Color UnlockedColor = new Color(1f, 1f, 1f, 1f);

    public void Start()
    {
        if (this.Level == GameLevel.Undefined)
        {
            this.SetUnlocked(false);
        }
        else
        {
            this.SetUnlocked(LevelManager.Instance.IsLevelWinned(this.Level));
        }

        this.Avatar.Appearance = LevelManager.Instance.HasSavedAvatarsForLevel(this.Level)
            ? LevelManager.Instance.GetAvatarForLevel(this.Level, this.AvatarIndex)
            : null;
    }

    private void SetUnlocked(bool isUnlocked)
    {
        this.Button.interactable = isUnlocked;
        this.Avatar.SetColor(isUnlocked ? this.UnlockedColor : this.LockedColor);
    }
}
