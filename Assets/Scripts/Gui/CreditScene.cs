using Hellmade.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScene : MonoBehaviour
{
    public AudioClip MusicClip;

    public void Start()
    {
        EazySoundManager.PlayMusic(this.MusicClip, 1.0f, false, false);
    }

    public void OnBackButtonClicked()
    {
        LevelManager.Instance.TransitionToScene("MainMenu");
    }
}
