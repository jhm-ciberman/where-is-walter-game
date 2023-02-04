using UnityEngine;

[System.Serializable]
public class BodyPreset
{
    public Sprite Sprite;

    public Sprite HandSprite;

    [Range(0, 1)]
    public float Probability = 100;
}
