using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogIntroSequence : MonoBehaviour
{
    public DialogBoxController DialogBoxController;

    private readonly string[] _messages = new [] {
        "¿Hola? ¿Hay alguien ahí?",
        "Al fin! Pensé que SE HABÍAN OLVIDADO DE MÍ.",
        "estoy buscando información sobre mi familia... pero no encuentro nada!",
        "no puedo irme hasta completar mi tarea. ¿puedes ayudarme?",
        "¡Se que tienes demasiada flojera como para hacer servicio a la comunidad!",
        "tarde! ya estás aquí, además ¿tienes algo más importante que hacer?",
        "¡Tal como pensaba! empecemos.",
        "Por cierto, aún no nos hemos presentado. Soy walter, ¡mucho gusto!",
    };

    public void Start()
    {
        this.DialogBoxController.DialogFinished += this.OnDialogFinished;
        this.DialogBoxController.AddMessages(this._messages);
        this.DialogBoxController.NextMessage();
    }

    private void OnDialogFinished()
    {
        SceneManager.LoadScene("MenuArbol");
    }
}
