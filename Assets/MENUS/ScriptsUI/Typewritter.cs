using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Typewritter : MonoBehaviour
{
    public TMP_Text text;
    public float delay = 0.1f;
    public string[] messages;
    private int currentMessage = 0;
    public GameObject W1,W2,W3,W4,MENUinteractivo,Textos_dialogos;
    public string SiguienteEscena;

    void Start()
    {
        text.text = "";
        StartCoroutine(Type());
    }

    void Update()
    {
        if(currentMessage == 0)
        {
            W1.SetActive(false);
            W2.SetActive(false);
            W3.SetActive(false);
            W4.SetActive(false);
        }
        else if(currentMessage == 1)
        {
            W1.SetActive(true);
            W2.SetActive(false);
            W3.SetActive(false);
            W4.SetActive(false);
        }
        else if(currentMessage == 2)
        {
            W1.SetActive(false);
            W2.SetActive(true);
            W3.SetActive(false);
            W4.SetActive(false);
        }
        else if (currentMessage == 3)
        {
            W1.SetActive(false);
            W2.SetActive(false);
            W3.SetActive(true);
            W4.SetActive(false);
        }
        else if (currentMessage == 4)
        {
            W1.SetActive(false);
            W2.SetActive(false);
            W3.SetActive(false);
            W4.SetActive(true);
        }
        else if(currentMessage == 5)
        {
            W1.SetActive(false);
            W2.SetActive(false);
            W3.SetActive(false);
            W4.SetActive(true);
        }else if(currentMessage == 6)
        {
            W1.SetActive(true);
            W2.SetActive(false);
            W3.SetActive(false);
            W4.SetActive(false);
        }
        else if(currentMessage == 7)
        {
            W1.SetActive(false);
            W2.SetActive(false);
            W3.SetActive(true);
            W4.SetActive(false);
        }
        else if(currentMessage == 8)
        {
            SceneManager.LoadScene(SiguienteEscena);
        }
    }

    IEnumerator Type()
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
