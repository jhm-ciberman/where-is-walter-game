using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GMRoom : MonoBehaviour
{
    public static GMRoom instance;
    public GameObject USB, PHONE;
    public string NombreObjeto;
    public GuiController guicontroller;
    public int Encontrados = 0;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        guicontroller.SetTargetObjects(new GameObject[]
        {
            USB, PHONE
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(Encontrados == 2)
        {
            SceneManager.LoadScene("MenuArbol");
        }
    }

    public void propfound(PropType propType)
    {
        if(propType == PropType.USB)
        {
            guicontroller.MarkTargetObjectAsFound(this.USB);
            Encontrados = Encontrados + 1;
        }else
        {
            guicontroller.MarkTargetObjectAsFound(this.PHONE);
            Encontrados = Encontrados + 1;
        }
    }

    
}
