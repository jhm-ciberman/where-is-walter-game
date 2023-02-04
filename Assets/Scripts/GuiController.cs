using UnityEngine;
using UnityEngine.UI;

public class GuiController : MonoBehaviour
{
    public Text RemainingTimeText;

    public Text GameOverText;

    public void Start()
    {
        this.GameOverText.gameObject.SetActive(false);
    }

    public void ShowGameOver()
    {
        this.GameOverText.gameObject.SetActive(true);
    }

    public void SetRemainingTime(float remainingTime)
    {
        this.RemainingTimeText.text = remainingTime.ToString("0");
    }
}
