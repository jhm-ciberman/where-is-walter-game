using UnityEngine;

public class GuiController : MonoBehaviour
{
    public void Start()
    {

    }

    public void ShowGameOver()
    {
        Debug.Log("Game Over");
    }

    public void SetRemainingTime(float remainingTime)
    {
        Debug.Log("Remaining time: " + remainingTime);
    }
}
