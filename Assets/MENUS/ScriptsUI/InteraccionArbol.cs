using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InteraccionArbol : MonoBehaviour
{
    public TMP_Text text;
    public float delay = 0.1f;
    public string[] messages;
    public int currentMessage = 0;
    public GameObject TreesMenuShadow;
    public bool Intro = true;
    public GameObject Dialogue;

    public void Start()
    {
        text.text = "";

        if (LevelManager.Instance.UnlockedLevelsCount <= 1)
        {
            StartCoroutine(Type());
        }

    }

    public void Update()
    {
        if (currentMessage == 3)
        {
            TreesMenuShadow.SetActive(true);

        }
        else if (currentMessage >= 4)
        {
            LevelManager.Instance.StartLevel("CUARTO");
        }

        if (Intro == false)
        {
            Dialogue.SetActive(false);
        }
    }

    private IEnumerator Type()
    {
        while (true)
        {
            string message = messages[currentMessage];
            foreach (char letter in message.ToCharArray())
            {
                text.text += letter;
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0));
            text.text = "";
            currentMessage = (currentMessage + 1) % messages.Length;
        }
    }
}
