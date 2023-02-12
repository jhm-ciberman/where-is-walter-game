using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
