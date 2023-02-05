using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BodyPartsRepository", menuName = "BodyPartsRepository", order = 1)]
public class BodyPartsRepository : ScriptableObject
{
    public List<BodyPreset> Bodies;

    public List<FacePreset> Faces;

    public List<ClothesPreset> Clothes;

    public List<AccessoryPreset> Accessories;


    private void ValidateList<T>(List<T> list, string listName) where T : IEquatable<T>
    {
        if (list.Count == 0)
        {
            throw new Exception($"List {listName} is empty (in {this.name})");
        }

        var set = new HashSet<T>(list);
        if (list.Count != set.Count)
        {
            throw new Exception($"List {listName} contains duplicates (in {this.name})");
        }
    }

    public void Validate()
    {
        ValidateList(Bodies, nameof(Bodies));
        ValidateList(Faces, nameof(Faces));
        ValidateList(Clothes, nameof(Clothes));
        ValidateList(Accessories, nameof(Accessories));

        int totalCount = this.Bodies.Count * this.Faces.Count * this.Clothes.Count * this.Accessories.Count;

        Debug.Log($"Total combinations: {totalCount} (in {this.name})");

        if (totalCount < 2)
        {
            throw new Exception($"Too few combinations, only {totalCount} (in {this.name})");
        }
    }
}
