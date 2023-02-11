using System;
using System.Collections.Generic;
using UnityEngine;
public class DialogTreeSequence : MonoBehaviour
{
    public DialogBoxController DialogBoxController;

    private readonly Dictionary<GameLevel, string[]> _messages = new Dictionary<GameLevel, string[]>()
    {
        [GameLevel.Cuarto] = new [] {
            "¿Hola? ¿Hay alguien ahí?",
            "Al fin! Pensé que SE HABÍAN OLVIDADO DE MÍ.",
            "estoy buscando información sobre mi familia... pero no encuentro nada!",
            "no puedo irme hasta completar mi tarea. ¿puedes ayudarme?",
            "¡Se que tienes demasiada flojera como para hacer servicio a la comunidad!",
            "tarde! ya estás aquí, además ¿tienes algo más importante que hacer?",
            "¡Tal como pensaba! empecemos.",
            "[ShowTreeUI]",
            "Por cierto, aún no nos hemos presentado. Soy walter, ¡mucho gusto!",
            "Este es mi árbol genealógico!",
            "Como puedes ver hay muchos familiares por descubrir.",
            "Antes de empezar nuestro viaje, necesito algunos ítems.",
            "[ShowOverlay]",
            "¡Creo que tengo todo en mi cuarto! Elige el recuadro para empezar.",
        },
        [GameLevel.Disco] = new [] {
            "¡Eso fue mejor de lo que esperaba! nunca dudé en tus habilidades!",
            "...bueno, tal vez un poco jeje",
            "[ShowTreeUI]",
            "Ahora tenemos que encontrar a mis padres, estoy seguro que se fueron de fiesta.",
        },
        [GameLevel.BigBen] = new [] {
            "¡Oh wow! En serio podemos lograrlo, pensé que iba a ser imposible.",
            "[ShowTreeUI]",
            "Es hora de encontrar a mis abuelos, oh yes!",
        },
        [GameLevel.Bar] = new [] {
            "Ok, esto va en serio, sólo nos queda encontrar a mis bisabuelos. Estamos muy cerca!",
            "[ShowTreeUI]",
            "No pude conocerlos, pero mis padres siempre me cuentan lo mucho que les gustaba ir a bares",
            "Último esfuerzo, nosotros podemos!",
        },
        [GameLevel.Ending] = new [] {
            "[ShowTreeUI]",
            "¡Yay! Gracias a ti pude recuperar mi árbol genealógico. ¡Eres lo más!",
        },
    };

    public GameObject BackgroundOverlayObject;

    public GameObject TreeUIObject;

    public void Start()
    {
        this.TreeUIObject.SetActive(false);

        this.DialogBoxController.DialogFinished += this.OnDialogFinished;
        this.DialogBoxController.MessageStarted += this.OnMessageChanged;

        GameLevel level = LevelManager.Instance.GetFirstUnlockedLevel();

        if (!this._messages.ContainsKey(level))
        {
            throw new Exception($"Level {level} not found in messages dictionary");
        }

        var messages = this._messages[level];

        this.DialogBoxController.AddMessages(messages);
        this.DialogBoxController.NextMessage();
    }

    private void OnMessageChanged()
    {
        switch (this.DialogBoxController.CurrentMessage)
        {
            case "[HideTreeUI]": this.HideTreeUICommand(); break;
            case "[ShowTreeUI]": this.ShowTreeUICommand(); break;
            case "[ShowOverlay]": this.ShowOverlayCommand(); break;
        }
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