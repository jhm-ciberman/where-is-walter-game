using UnityEngine;

[System.Serializable]
public class FacePreset
{
    public Sprite Sprite;


    [Range(0, 1)]
    public float Probability = 1;
}
