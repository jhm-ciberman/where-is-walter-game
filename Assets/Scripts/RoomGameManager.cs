using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class RoomGameManager : MonoBehaviour
{
    public static RoomGameManager Instance;
    public RectTransform UsbGameObject;
    public RectTransform PhoneGameObject;
    private bool _usbFound = false;
    private bool _phoneFound = false;
    public GuiController GuiController;
    private bool _isPlaying = true;
    private float _remainingTime;
    public float TotalTime = 60f;
    public int Attempts = 4;

    public void Start()
    {
        Instance = this;
        this.GuiController.SetTargetObjects(new[]
        {
            UsbGameObject,
            PhoneGameObject,
        });

        this._remainingTime = this.TotalTime;

        this.GuiController.gameObject.SetActive(true);
        this.GuiController.SetRemainingAttempts(this.Attempts);
    }

    public void Update()
    {
        if (this._isPlaying && this._usbFound && this._phoneFound)
        {
            this.WinLevel();
        }

        this._remainingTime -= Time.deltaTime;

        if (this._remainingTime < 0)
        {
            this._remainingTime = 0;
            this.LooseLevel();
        }

        this.GuiController.SetRemainingTime(this._remainingTime);

    }

    private void WinLevel()
    {
        this._isPlaying = false;
        LevelManager.Instance.NotifyLevelCompleted();
        this.GuiController.ShowGameWon();

        DOVirtual.DelayedCall(3f, () => SceneManager.LoadScene("MenuArbol"));
    }

    private void LooseLevel()
    {
        this._isPlaying = false;
        this.GuiController.ShowGameOver();

        DOVirtual.DelayedCall(3f, () => SceneManager.LoadScene("MenuArbol"));
    }

    public void Propfound(PropType propType)
    {
        if (propType == PropType.USB)
        {
            this.GuiController.MarkTargetObjectAsFound(this.UsbGameObject);
            this._usbFound = true;
        }
        else if (propType == PropType.PHONE)
        {
            this.GuiController.MarkTargetObjectAsFound(this.PhoneGameObject);
            this._phoneFound = true;
        }
        else
        {
            this.Attempts--;
            this.GuiController.SetRemainingTime(this._remainingTime);
            this.GuiController.SetRemainingAttempts(this.Attempts);

            if (this.Attempts == 0)
            {
                LooseLevel();
            }
        }
    }
}
