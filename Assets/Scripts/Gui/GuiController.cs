using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Hellmade.Sound;

public class GuiController : MonoBehaviour
{
    public TextMeshProUGUI RemainingTimeText;

    public Transform OutOfAttemptsDisplay;

    public Transform OutOfTimeDisplay;

    public Transform WinDisplay;

    public TextMeshProUGUI AttemptsText;

    public RectTransform StatsPanel;

    public AudioClip CorrectAnswerSound;

    public AudioClip WrongAnswerSound;

    public AudioClip LooseSound;

    public GuiThumbnailComponent[] TargetsThumbnails;

    public enum LooseReason
    {
        OutOfTime,
        OutOfAttempts,
    }

    public void Start()
    {
        this.OutOfAttemptsDisplay.gameObject.SetActive(false);
        this.OutOfTimeDisplay.gameObject.SetActive(false);
        this.WinDisplay.gameObject.SetActive(false);
    }

    public void ShowGameOver(LooseReason reason)
    {
        EazySoundManager.StopAllMusic();
        EazySoundManager.PlayMusic(this.LooseSound);
        switch (reason)
        {
            case LooseReason.OutOfAttempts:
                this.ShowOutOfAttempts();
                break;
            case LooseReason.OutOfTime:
                this.ShowOutOfTime();
                break;
            default:
                throw new System.Exception("Unknown reason");
        }
    }

    public void WrongAnswer()
    {
        EazySoundManager.PlaySound(this.WrongAnswerSound, volume: 0.5f);
    }

    private void ShowOutOfTime()
    {
        this.OutOfTimeDisplay.gameObject.SetActive(true);

        this.AnimateSlidingUp(this.OutOfTimeDisplay.transform);
    }

    private void ShowOutOfAttempts()
    {
        this.OutOfAttemptsDisplay.gameObject.SetActive(true);

        this.AnimateSlidingUp(this.OutOfAttemptsDisplay.transform);
    }

    public void SetRemainingTime(float remainingTime)
    {
        this.RemainingTimeText.text = remainingTime.ToString("00");
    }

    internal void ShowGameWon()
    {
        this.WinDisplay.gameObject.SetActive(true);

        this.AnimateSlidingUp(this.WinDisplay.transform);
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

        EazySoundManager.PlaySound(this.CorrectAnswerSound, volume: 2.0f);
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

        EazySoundManager.PlaySound(this.CorrectAnswerSound, volume: 2.0f);
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
