using System;
using System.Collections.Generic;
using DG.Tweening;
using Hellmade.Sound;
using UnityEngine;

public class DialogTreeSequence : MonoBehaviour
{
    public DialogBoxController DialogBoxController;

    private readonly Dictionary<GameLevel, string[]> _messages = new Dictionary<GameLevel, string[]>()
    {
        [GameLevel.Cuarto] = new [] {
            "Hello? Is anyone there?",
            "Finally! I thought I was forgotten.",
            "I'm looking for information about my family... but I can't find anything!",
            "I can't leave until I complete my task. Can you help me?",
            "[ShowTreeUI]",
            "By the way, I haven't introduced myself yet. I'm walter, nice to meet you!",
            "This is my family tree!",
            "As you can see, there are many relatives to discover.",
            "Before we start our journey, I need some items.",
            "[ShowOverlay]",
            "I think I have everything in my room!",
            "I will show you two items and you need to find them in my room.",
            "Let's start!"
        },
        [GameLevel.Disco] = new [] {
            "That was better than I expected! I never doubted your abilities!",
            "...well, maybe a little hehe",
            "[ShowTreeUI]",
            "Now we have to find my parents.",
            "I'm sure they went to a party.",
        },
        [GameLevel.BigBen] = new [] {
            "Oh wow! We can really do it, I thought it was impossible.",
            "[ShowTreeUI]",
            "It's time to find my grandparents, oh yes!",
        },
        [GameLevel.Bar] = new [] {
            "Ok, this is serious, we just have to find my great-grandparents. We're so close!",
            "[ShowTreeUI]",
            "I never got to meet them, but...",
            "...my parents always told me how much they liked going to bars",
            "Last effort, we can do it!",
        },
        [GameLevel.Ending] = new [] {
            "[ShowTreeUI]",
            "Wow! You found all my relatives!",
            "[ShowConfetti]",
            "Yay! Thanks to you I was able to recover my family tree. You're the best!",
            "Thanks for playing!",
            "Without you this game wouldn't be possible.",
            "This mini game was created for the 2021 Global Game Jam.",
            "[ShowCredits]",
        },
    };

    public GameObject BackgroundOverlayObject;

    public GameObject TreeUIObject;

    public GameObject ConfettiObject;

    public AudioClip MusicClip;

    public void Start()
    {
        this.TreeUIObject.SetActive(false);
        this.ConfettiObject.SetActive(false);

        this.DialogBoxController.DialogFinished += this.OnDialogFinished;
        this.DialogBoxController.MessageStarted += this.OnMessageChanged;

        GameLevel level = LevelManager.Instance.GetNextLevelToPlay();

        if (level == GameLevel.Undefined)
        {
            level = GameLevel.Ending;
        }

        if (!this._messages.ContainsKey(level))
        {
            throw new Exception($"Level {level} not found");
        }

        var messages = this._messages[level];

        this.DialogBoxController.AddMessages(messages);
        this.DialogBoxController.NextMessage();

        EazySoundManager.PlayMusic(this.MusicClip, 1.0f, true, false);
    }

    private void OnMessageChanged()
    {
        switch (this.DialogBoxController.CurrentMessage)
        {
            case "[HideTreeUI]": this.HideTreeUICommand(); break;
            case "[ShowTreeUI]": this.ShowTreeUICommand(); break;
            case "[ShowOverlay]": this.ShowOverlayCommand(); break;
            case "[ShowConfetti]": this.ShowConfettiCommand(); break;
            case "[ShowCredits]": this.ShowCreditsCommand(); break;
        }
    }

    private void ShowCreditsCommand()
    {
        LevelManager.Instance.ResetProgress();
        LevelManager.Instance.TransitionToScene("Credits");
        this.DialogBoxController.gameObject.SetActive(false);
    }

    private void ShowConfettiCommand()
    {
        this.ConfettiObject.transform.localScale = Vector3.zero;
        this.ConfettiObject.SetActive(true);
        this.ConfettiObject.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);

        this.DialogBoxController.NextMessage();
    }

    private void ShowOverlayCommand()
    {
        this.BackgroundOverlayObject.SetActive(true);
        this.DialogBoxController.NextMessage();
    }

    private void ShowTreeUICommand()
    {
        this.BackgroundOverlayObject.SetActive(false);
        this.TreeUIObject.SetActive(true);
        this.DialogBoxController.NextMessage();
    }

    private void HideTreeUICommand()
    {
        this.TreeUIObject.SetActive(false);
        this.DialogBoxController.NextMessage();
    }

    private void OnDialogFinished()
    {
        this.BackgroundOverlayObject.SetActive(false);
        LevelManager.Instance.LoadNextLevel();
    }
}
