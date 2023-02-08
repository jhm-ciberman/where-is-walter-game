using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogBoxController : MonoBehaviour
{
    public event System.Action DialogFinished;

    public event System.Action MessageStarted;

    public event System.Action MessageFinished;

    public float CharactersPerMinute = 60f;

    private readonly List<string> _messages = new List<string>();

    private bool _isPlayingMessage = false;

    public int CurrentMessageIndex { get; private set; } = -1;
    public int CurrentCharacterIndex { get; private set; } = 0;
    public string CurrentMessage { get; private set; } = "";
    public float CurrentWaitTimer { get; private set; } = 0f;

    public TextMeshProUGUI TextBoxText;
    public GameObject DialogBoxObject;
    public GameObject ContinueArrowObject;

    public void Start()
    {
        this.TextBoxText.text = "";
    }

    public void AddMessage(string message)
    {
        this._messages.Add(message);
    }

    public void AddMessages(IEnumerable<string> messages)
    {
        this._messages.AddRange(messages);
    }

    public void NextMessage()
    {
        int index = this.CurrentMessageIndex + 1;
        if (index >= this._messages.Count)
        {
            this.OnDialogFinished();
            return;
        }

        this.CurrentMessageIndex = index;
        this.CurrentCharacterIndex = 0;
        string message = this._messages[this.CurrentMessageIndex];
        this.ContinueArrowObject.SetActive(false);


        this.CurrentMessage = message;
        this._isPlayingMessage = true;
        this.UpdateText();
        this.MessageStarted?.Invoke();
    }

    public void SkipMessage()
    {
        this.CurrentCharacterIndex = this.CurrentMessage.Length;
        this.UpdateText();
    }

    private void UpdateText()
    {
        this.TextBoxText.text = this.CurrentMessage[..this.CurrentCharacterIndex];
    }

    private void OnDialogFinished()
    {
        this.ContinueArrowObject.SetActive(false);
        this.DialogBoxObject.SetActive(false);
        this.DialogFinished?.Invoke();
    }

    public void Update()
    {
        if (this.CurrentCharacterIndex < this.CurrentMessage.Length)
        {
            this.CurrentWaitTimer += Time.deltaTime;
            float waitTime = 1f / (this.CharactersPerMinute / 60f);
            if (this.CurrentWaitTimer >= waitTime)
            {
                this.CurrentWaitTimer -= waitTime;
                this.CurrentCharacterIndex++;
                this.UpdateText();
            }
        }
        else
        {
            this.ContinueArrowObject.SetActive(true);

            if (this._isPlayingMessage)
            {
                this._isPlayingMessage = false;
                this.MessageFinished?.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            if (this.CurrentCharacterIndex < this.CurrentMessage.Length)
            {
                this.SkipMessage();
            }
            else
            {
                this.NextMessage();
            }
        }
    }
}
