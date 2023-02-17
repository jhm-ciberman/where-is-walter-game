using Hellmade.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip MusicClip;

    public void Start()
    {
        EazySoundManager.PlayMusic(this.MusicClip, 1.0f, true, false);
    }

    public void OnStartButtonClicked()
    {
        LevelManager.Instance.TransitionToScene("TreeMenu");
    }

    public void OnCreditsButtonClicked()
    {
        LevelManager.Instance.TransitionToScene("Credits");
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
