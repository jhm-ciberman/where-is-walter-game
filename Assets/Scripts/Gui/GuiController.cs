using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiController : MonoBehaviour
{
    public Text RemainingTimeText;

    public Text GameOverText;

    public GuiThumbnailComponent[] TargetsThumbnails;

    public void Start()
    {
        this.GameOverText.gameObject.SetActive(false);
    }

    public void ShowGameOver()
    {
        this.GameOverText.gameObject.SetActive(true);
    }

    public void SetRemainingTime(float remainingTime)
    {
        this.RemainingTimeText.text = remainingTime.ToString("0");
    }

    internal void ShowGameWon()
    {
        Debug.Log("Game won");
    }


    internal void MarkTargetAsFound(AvatarAppearance appearance)
    {
        foreach (var thumbnail in this.TargetsThumbnails)
        {
            if (thumbnail.Appearance == appearance)
            {
                thumbnail.MarkAsFound();
            }
        }
    }

    public void SetTargets(IEnumerable<AvatarAppearance> targets)
    {
        var i = 0;
        foreach (var target in targets)
        {
            this.TargetsThumbnails[i].Appearance = target;
            i++;
        }

        for (; i < this.TargetsThumbnails.Length; i++)
        {
            this.TargetsThumbnails[i].gameObject.SetActive(false);
        }
    }
}
