using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nextscenewbut : MonoBehaviour
{

    public string NextSceneName = "MenuArbol";

    public void NextScene()
    {
        SceneManager.LoadScene(this.NextSceneName);
    }
}
