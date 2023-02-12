using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropType
{
    USB,
    PHONE,
    SPEAKER,
    WALLET,
    DISK,
    MUG
}

public class RoomProp : MonoBehaviour
{
    public PropType PropType;

    public GameObject ExplosionParticlePrefab;

    public GameObject FoundParticlePrefab;

    public void OnMouseDown()
    {
        RoomGameManager.Instance.Propfound(this);
    }

    public void Explode()
    {
        GameObject.Instantiate(this.ExplosionParticlePrefab, this.transform.position, Quaternion.identity);
        GameObject.Destroy(this.gameObject, .25f);

        var pos = this.transform.localPosition;
        this.transform.localPosition = pos + Vector3.forward * 0.1f; // move forward a bit so its hidden behind the explode particle
    }

    public void Found()
    {
        var go = GameObject.Instantiate(this.FoundParticlePrefab, this.transform.position, Quaternion.identity);
        go.transform.localScale = Vector3.one * 2f;
        GameObject.Destroy(this.gameObject, .25f);

        var pos = this.transform.localPosition;
        this.transform.localPosition = pos + Vector3.forward * 0.1f; // move forward a bit so its hidden behind the found particle
    }
}
