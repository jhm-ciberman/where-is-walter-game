using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nextscenewbut : MonoBehaviour
{

    public void NextScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
