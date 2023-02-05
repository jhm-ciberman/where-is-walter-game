using System;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartsController : MonoBehaviour
{
    public BodyPartsRepository Repository;

    private readonly System.Random _random;

    private readonly WeightedRandom<BodyPreset> _bodyRandom = new WeightedRandom<BodyPreset>();

    private readonly WeightedRandom<FacePreset> _headRandom = new WeightedRandom<FacePreset>();

    private readonly WeightedRandom<ClothesPreset> _clothesRandom = new WeightedRandom<ClothesPreset>();

    private readonly WeightedRandom<AccessoryPreset> _accessoryRandom = new WeightedRandom<AccessoryPreset>();


    private readonly List<AvatarAppearance> _excluded = new List<AvatarAppearance>();

    public BodyPartsController()
    {
        this._random = new System.Random();
    }

    public void Awake()
    {
        if (this.Repository == null)
        {
            throw new Exception("Repository is not set");
        }

        this.Repository.Validate();

        foreach (var body in this.Repository.Bodies)
        {
            this._bodyRandom.Add(body, body.Probability);
        }

        foreach (var head in this.Repository.Faces)
        {
            this._headRandom.Add(head, head.Probability);
        }

        foreach (var clothes in this.Repository.Clothes)
        {
            this._clothesRandom.Add(clothes, clothes.Probability);
        }

        foreach (var accessory in this.Repository.Accessories)
        {
            this._accessoryRandom.Add(accessory, accessory.Probability);
        }
    }

    private AvatarAppearance GetNextAppearance()
    {
        var body = this._bodyRandom.Next();
        var head = this._headRandom.Next();
        var clothes = this._clothesRandom.Next();
        var accessory = this._accessoryRandom.Next();

        return new AvatarAppearance(body, head, clothes, accessory);
    }

    public AvatarAppearance GetRandomAppearance()
    {
        var appearance = this.GetNextAppearance();

        while (this._excluded.Contains(appearance))
        {
            appearance = this.GetNextAppearance();
        }

        //this._excluded.Add(appearance);

        return appearance;
    }

    public void ExcludeAppearance(AvatarAppearance appearance)
    {
        this._excluded.Add(appearance);
    }

    public void ResetExcludedAppearances()
    {
        this._excluded.Clear();
    }
}
