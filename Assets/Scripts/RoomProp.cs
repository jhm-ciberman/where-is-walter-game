using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropType
{
    USB, PHONE,
}

public class RoomProp : MonoBehaviour
{
    public PropType propType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        GMRoom.instance.propfound(this.propType);
    }
}
