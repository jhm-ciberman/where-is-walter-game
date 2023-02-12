using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class RoomGameManager : MonoBehaviour
{
    public static RoomGameManager Instance;
    public RectTransform UsbGameObject;
    public RectTransform PhoneGameObject;
    public CameraController CameraController;
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
            this.LooseLevel(GuiController.LooseReason.OutOfTime);
        }

        this.GuiController.SetRemainingTime(this._remainingTime);

    }

    private void WinLevel()
    {
        this._isPlaying = false;
        var level = LevelManager.Instance.GetCurrentLevel();
        LevelManager.Instance.NotifyLevelCompleted(level);
        this.GuiController.ShowGameWon();

        DOVirtual.DelayedCall(3f, () => SceneManager.LoadScene("TreeMenu"));
    }

    private void LooseLevel(GuiController.LooseReason reason)
    {
        this._isPlaying = false;
        this.GuiController.ShowGameOver(reason);

        DOVirtual.DelayedCall(3f, () => LevelManager.Instance.RestartLevel());
    }

    public void Propfound(RoomProp prop)
    {
        this.CameraController.FocusOn(prop.transform.position, 0f, 2f);

        DOVirtual.DelayedCall(0.5f, () => this.HandlePropFound(prop));
    }

    private void HandlePropFound(RoomProp prop)
    {
        if (prop.PropType == PropType.USB)
        {
            this.GuiController.MarkTargetObjectAsFound(this.UsbGameObject);
            this._usbFound = true;
            prop.Found();
        }
        else if (prop.PropType == PropType.PHONE)
        {
            this.GuiController.MarkTargetObjectAsFound(this.PhoneGameObject);
            this._phoneFound = true;
            prop.Found();
        }
        else
        {
            prop.Explode();
            this.Attempts--;
            this.GuiController.SetRemainingTime(this._remainingTime);
            this.GuiController.SetRemainingAttempts(this.Attempts);

            if (this.Attempts == 0)
            {
                LooseLevel(GuiController.LooseReason.OutOfAttempts);
            }
        }
    }
}
