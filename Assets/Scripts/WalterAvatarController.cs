using UnityEngine;

public class WalterAvatarController : MonoBehaviour
{
    public GameObject IdleObject;

    public GameObject TalkingMouthOpenObject;

    public GameObject TalkingMouthClosedObject;

    public GameObject SmilingObject;

    private GameObject _currentObject;


    private float _mouthCountdown = 0f;

    private float _smileCountdown = 0f;

    public float TalkingInterval = 0.1f;

    public float SmilingInterval = 1.0f; // Only when not talking

    public float NonSmilingInterval = 4.5f; // Only when not talking

    public bool IsTalking { get; set; } = false;

    private bool _isMouthOpen = false;

    private bool _isSmiling = false;

    public DialogBoxController DialogBoxController;

    public void Start()
    {
        this.SetObject(this.IdleObject);

        if (this.DialogBoxController != null)
        {
            this.DialogBoxController.MessageStarted += this.OnMessageStarted;
            this.DialogBoxController.MessageFinished += this.OnMessageFinished;

            if (this.DialogBoxController.CurrentMessageIndex >= 0)
            {
                this.OnMessageStarted();
            }
        }

    }

    public void OnDestroy()
    {
        if (this.DialogBoxController != null)
        {
            this.DialogBoxController.MessageStarted -= this.OnMessageStarted;
            this.DialogBoxController.MessageFinished -= this.OnMessageFinished;
        }
    }

    private void OnMessageFinished()
    {
        this.IsTalking = false;
        this._isSmiling = true;  // Always smile when done talking :)
        this._smileCountdown = this.SmilingInterval;
    }

    private void OnMessageStarted()
    {
        this.IsTalking = true;
        this._isMouthOpen = true; // Always start talking with mouth open
        this._mouthCountdown = this.TalkingInterval;
    }

    private void SetObject(GameObject obj)
    {
        this._currentObject?.SetActive(false);
        this._currentObject = obj;
        this._currentObject.SetActive(true);
    }


    public void Update()
    {
        if (this.IsTalking)
        {
            this._mouthCountdown -= Time.deltaTime;
            if (this._mouthCountdown <= 0f)
            {
                this._isMouthOpen = !this._isMouthOpen;
                this._mouthCountdown = this.TalkingInterval;
            }

            this.SetObject(this._isMouthOpen ? this.TalkingMouthOpenObject : this.TalkingMouthClosedObject);
        }
        else
        {
            this._smileCountdown -= Time.deltaTime;
            if (this._smileCountdown <= 0f)
            {
                this._isSmiling = !this._isSmiling;

                this._smileCountdown = this._isSmiling ? this.SmilingInterval : this.NonSmilingInterval;
            }

            this.SetObject(this._isSmiling ? this.SmilingObject : this.IdleObject);
        }

    }

}
