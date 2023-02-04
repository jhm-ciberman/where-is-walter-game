using System;
using UnityEngine;

public class AvatarAppearance : IEquatable<AvatarAppearance>
{
    public BodyPreset Body { get; private set; }

    public FacePreset Face { get; private set; }

    public ClothesPreset Clothes { get; private set; }
    public AccessoryPreset Accessory { get; private set; }


    public AvatarAppearance(BodyPreset body, FacePreset face, ClothesPreset clothes, AccessoryPreset accessory)
    {
        this.Body = body;
        this.Face = face;
        this.Clothes = clothes;
        this.Accessory = accessory;
    }


    public bool Equals(AvatarAppearance other)
    {
        if (other == null)
        {
            return false;
        }

        return this.Body == other.Body
            && this.Face == other.Face
            && this.Accessory == other.Accessory
            && this.Clothes == other.Clothes;
    }
}
