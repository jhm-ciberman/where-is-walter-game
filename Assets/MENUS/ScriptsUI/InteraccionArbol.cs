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

    void Start()
    {
        text.text = "";
        StartCoroutine(Type());
    }

    void Update()
    {
        if (LevelManager.Instance.UnlockedLevelsCount > 0)
        {
            Intro = false;
        }

        if (currentMessage == 3)
        {
            TreesMenuShadow.SetActive(true);
            
        }
       else if(currentMessage >= 4)
       {
            SceneManager.LoadScene("CUARTO");
       }

        if(Intro == false)
        {
            Dialogue.SetActive(false);
        }

    }

    IEnumerator Type()
    {
        while (true)
        {
            if (Intro == true)
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
}
