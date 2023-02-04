using UnityEngine;

[System.Serializable]
public class AccessoryPreset
{
    public Sprite Sprite;

    [Range(0, 1)]
    public float Probability = 1f;
}
