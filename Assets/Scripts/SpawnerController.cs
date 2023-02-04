using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    private readonly List<AvatarController> _avatars = new List<AvatarController>();

    public AvatarController AvatarPrefab;

    public List<SpawnLane> Lanes;

    public List<MovePreset> MovePresets;

    private System.Random _random = new System.Random();

    public float DeltaTimeScaleRange = 0.1f;

    public void Start()
    {
        this._avatars.Clear();
    }

    public void Spawn(AvatarAppearance appearance, SpawnLane lane, float laneProgress, MovePreset movePreset)
    {
        var position = lane.Start.position;
        var direction = (lane.End.position - lane.Start.position).normalized;
        var distance = Vector3.Distance(lane.Start.position, lane.End.position);
        position += direction * distance * laneProgress;
        position.z = 0;
        var avatar = Instantiate(this.AvatarPrefab, position, Quaternion.identity, lane.transform);
        this._avatars.Add(avatar);
        avatar.Appearance = appearance;
        avatar.transform.position = position;
        avatar.transform.localScale = Vector3.one * lane.AvatarSize;
        avatar.SetLane(lane, laneProgress);
        avatar.MovePreset = movePreset;
        avatar.InitializeRandom(this._random);
        avatar.SetSortingLayer(lane.GetSortingLayerID());
        avatar.SetSortingOrder(this._avatars.Count);
    }

    public void SpawnRandom(AvatarAppearance appearance)
    {
        var lane = this.Lanes[this._random.Next(this.Lanes.Count)];
        var movePreset = this.MovePresets[this._random.Next(this.MovePresets.Count)];
        var progress = (float)this._random.NextDouble();
        this.Spawn(appearance, lane, progress, movePreset);
    }
}
