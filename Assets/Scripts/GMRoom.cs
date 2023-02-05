using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GMRoom : MonoBehaviour
{
    public static GMRoom Instance;
    public GameObject USB, PHONE;
    public string NombreObjeto;
    public GuiController GuiController;
    public int Encontrados = 0;
    public bool IsPlaying = true;
    public float RemainingTime;
    public float TotalTime = 60f;
    public int Attempts = 4;

    // Start is called before the first frame update
    public void Start()
    {
        Instance = this;
        GuiController.SetTargetObjects(new GameObject[]
        {
            USB, PHONE
        });

        RemainingTime = TotalTime;

        GuiController.SetRemainingAttempts(Attempts);
    }

    // Update is called once per frame
    public void Update()
    {
        if (this.Encontrados == 2 && IsPlaying)
        {
            WinLevel();
        }

        RemainingTime -= Time.deltaTime;

        if (RemainingTime < 0)
        {
            RemainingTime = 0;
            LooseLevel();
        }

        GuiController.SetRemainingTime(RemainingTime);

    }

    private void WinLevel()
    {
        IsPlaying = false;
        LevelManager.Instance.NotifyLevelCompleted();
        this.GuiController.ShowGameWon();
        // Load next scene
        DOVirtual.DelayedCall(3f, () =>
        {
            SceneManager.LoadScene("MenuArbol");
        });
    }

    private void LooseLevel()
    {
        IsPlaying = false;
        this.GuiController.ShowGameOver();

        // Load next scene
        DOVirtual.DelayedCall(3f, () =>
        {
            SceneManager.LoadScene("MenuArbol");
        });
    }

    public void Propfound(PropType propType)
    {
        if (propType == PropType.USB)
        {
            GuiController.MarkTargetObjectAsFound(this.USB);
            Encontrados++;
        }
        else if (propType == PropType.PHONE)
        {
            GuiController.MarkTargetObjectAsFound(this.PHONE);
            Encontrados++;
        }
        else
        {
            Attempts--;
            GuiController.SetRemainingTime(RemainingTime);

            if (Attempts == 0)
            {
                LooseLevel();
            }
        }
    }


}
