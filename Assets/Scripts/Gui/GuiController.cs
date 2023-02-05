using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuiController : MonoBehaviour
{
    public TextMeshProUGUI RemainingTimeText;

    public TextMeshProUGUI GameOverText;

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


    internal void MarkTargetAvatarAsFound(AvatarAppearance appearance)
    {
        foreach (var thumbnail in this.TargetsThumbnails)
        {
            if (thumbnail.Appearance == appearance)
            {
                thumbnail.MarkAsFound();
            }
        }
    }

    public void MarkTargetObjectAsFound(GameObject targetObject)
    {
        foreach (var thumbnail in this.TargetsThumbnails)
        {
            if (thumbnail.TargetObjectGameObject == targetObject)
            {
                thumbnail.MarkAsFound();
            }
        }
    }

    public void SetTargetAvatars(IEnumerable<AvatarAppearance> targets)
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

    public void SetTargetObjects(IEnumerable<GameObject> targets)
    {
        var i = 0;
        foreach (var target in targets)
        {
            this.TargetsThumbnails[i].TargetObjectGameObject = target;
            i++;
        }

        for (; i < this.TargetsThumbnails.Length; i++)
        {
            this.TargetsThumbnails[i].gameObject.SetActive(false);
        }
    }
}
