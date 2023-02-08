using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogTreeSequence : MonoBehaviour
{
    public DialogBoxController DialogBoxController;

    private readonly string[] _messages = new [] {
        "Este es mi árbol genealógico!",
        "Como puedes ver hay muchos familiares por descubrir.",
        "Antes de empezar nuestro viaje, necesito algunos ítems.",
        "[ShowOverlay]", // <-- This is a special message that will trigger the modal to show
        "¡Creo que tengo todo en mi cuarto! Elige el recuadro para empezar.",
    };

    public GameObject BackgroundOverlayObject;

    public void Start()
    {
        this.DialogBoxController.DialogFinished += this.OnDialogFinished;
        this.DialogBoxController.MessageStarted += this.OnMessageChanged;
        this.DialogBoxController.AddMessages(this._messages);
        this.DialogBoxController.NextMessage();
    }

    private void OnMessageChanged()
    {
        if (this.DialogBoxController.CurrentMessage == "[ShowOverlay]")
        {
            if (this.BackgroundOverlayObject == null)
            {
                throw new Exception("BackgroundOverlayObject is null");
            }
            this.BackgroundOverlayObject.SetActive(true);
            this.DialogBoxController.NextMessage();
        }
    }

    private void OnDialogFinished()
    {
        SceneManager.LoadScene("CUARTO");
    }
}
