using System;
using UnityEngine;

[System.Serializable]
public class FacePreset : IEquatable<FacePreset>
{
    public Sprite Sprite;

    [Range(0, 1)]
    public float Probability = 1;

    public bool Equals(FacePreset other)
    {
        return this.Sprite == other.Sprite;
    }

    public override int GetHashCode()
    {
        return this.Sprite.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as FacePreset);
    }
}
