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
}
