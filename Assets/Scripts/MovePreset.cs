using UnityEngine;

[CreateAssetMenu(fileName = "MovePreset", menuName = "MovePreset", order = 0)]
public class MovePreset : ScriptableObject
{
    public float Speed = 1f;

    public float BounceHeight = 0.11f; // Proportion of avatar size

    public float BounceSpeed = 2.37f;

    public float SwingAngle = 8f;
}
