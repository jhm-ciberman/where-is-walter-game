using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropType
{
    USB, PHONE, SPEAKER, WALLET, DISK, MUG
}

public class RoomProp : MonoBehaviour
{
    public PropType propType;

    public void OnMouseDown()
    {
        GMRoom.Instance.Propfound(this.propType);
    }
}
