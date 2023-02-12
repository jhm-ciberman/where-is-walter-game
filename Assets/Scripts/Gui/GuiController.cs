using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GuiController : MonoBehaviour
{
    public TextMeshProUGUI RemainingTimeText;

    public TextMeshProUGUI GameOverText;

    public TextMeshProUGUI WinText;

    public TextMeshProUGUI AttemptsText;

    public RectTransform StatsPanel;

    public GuiThumbnailComponent[] TargetsThumbnails;

    public void Start()
    {
        this.GameOverText.gameObject.SetActive(false);
        this.WinText.gameObject.SetActive(false);
    }

    public void ShowGameOver()
    {
        this.GameOverText.gameObject.SetActive(true);

        this.AnimateSlidingUp(this.GameOverText.transform);
    }

    public void SetRemainingTime(float remainingTime)
    {
        this.RemainingTimeText.text = remainingTime.ToString("00");
    }

    internal void ShowGameWon()
    {
        this.WinText.gameObject.SetActive(true);

        this.AnimateSlidingUp(this.WinText.transform);
    }

    private void AnimateSlidingUp(Transform transform)
    {
        Vector3 position = transform.localPosition;
        transform.localPosition -= Vector3.up * 1000;
        DOTween.To(
            () => transform.localPosition,
            x => transform.localPosition = x,
            position,
            0.5f
        ).SetEase(Ease.OutBack);
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

    public void MarkTargetObjectAsFound(RectTransform targetObject)
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

    public void SetTargetObjects(IEnumerable<RectTransform> targets)
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

    internal void SetRemainingAttempts(int attempts)
    {
        this.AttemptsText.text = attempts.ToString("00");
    }

    private bool _isCutscene = false;

    internal void SetCutsceneMode(bool activated)
    {
        if (this._isCutscene == activated) return;

        this._isCutscene = activated;
        this.StatsPanel.gameObject.SetActive(!activated);
    }
}
