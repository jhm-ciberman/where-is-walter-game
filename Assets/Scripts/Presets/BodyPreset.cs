using System;
using UnityEngine;

[System.Serializable]
public class BodyPreset : IEquatable<BodyPreset>
{
    public Sprite Sprite;

    public Sprite HandSprite;

    [Range(0, 1)]
    public float Probability = 100;

    public bool Equals(BodyPreset other)
    {
        return this.Sprite == other.Sprite;
    }

    public override int GetHashCode()
    {
        return this.Sprite.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as BodyPreset);
    }
}
