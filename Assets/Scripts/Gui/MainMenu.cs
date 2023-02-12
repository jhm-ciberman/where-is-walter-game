using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("TreeMenu");
    }

    public void OnCreditsButtonClicked()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
