using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GMRoom : MonoBehaviour
{
    public static GMRoom instance;
    public GameObject USB, PHONE;
    public string NombreObjeto;
    public GuiController guicontroller;
    public int Encontrados = 0;
    public bool isPlaying = true;
    public float remainingTime;
    public float TotalTime;
    public int Attempts = 4;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        guicontroller.SetTargetObjects(new GameObject[]
        {
            USB, PHONE
        });

        remainingTime = TotalTime;

        guicontroller.SetRemainingAttempts(Attempts);
    }

    // Update is called once per frame
    void Update()
    {
       
        if(Encontrados == 2 && isPlaying = true)
        {
            WinLevel();
        }

        remainingTime -= Time.deltaTime;
        if(remainingTime < 0)
        {
            remainingTime = 0;
            LooseLevel();
        }

        guicontroller.SetRemainingTime(remainingTime);

    }

    private void WinLevel()
    {
        isPlaying = false;
        LevelManager.Instance.NotifyLevelCompleted();
        this.GuiController.ShowGameWon();
        // Load next scene
        DOVirtual.DelayedCall(3f, () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuArbol");
        });
    }

    private void LooseLevel()
    {
        isPlaying = false;
        this._gameState = GameState.GameOver;
        this.GuiController.ShowGameOver();

        // Load next scene
        DOVirtual.DelayedCall(3f, () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuArbol");
        });
    }

    public void propfound(PropType propType)
    {
        if(propType == PropType.USB)
        {
            guicontroller.MarkTargetObjectAsFound(this.USB);
            Encontrados = Encontrados + 1;
        }
        else if(propType == propType.PHONE)
        {
            guicontroller.MarkTargetObjectAsFound(this.PHONE);
            Encontrados = Encontrados + 1;
        }
        else
        {
            Attempts--;
            guicontroller.SetRemainingTime(remainingTime);

            if(Attempts == 0)
            {
                LooseLevel();
            }
        }
    }

    
}
