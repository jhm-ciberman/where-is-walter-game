using UnityEngine;

[System.Serializable]
public class ClothesPreset
{
    public Sprite Sprite;

    [Range(0, 1)]
    public float Probability = 1;
}
