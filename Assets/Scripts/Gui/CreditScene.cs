using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScene : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
