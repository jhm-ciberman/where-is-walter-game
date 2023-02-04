using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    private readonly List<AvatarController> _avatars = new List<AvatarController>();

    public AvatarController AvatarPrefab;

    public List<SpawnLane> Lanes;

    private System.Random _random = new System.Random();

    public void Start()
    {
        this._avatars.Clear();
    }

    public void Spawn(AvatarAppearance appearance, SpawnLane lane, float laneProgress)
    {
        var position = lane.Start.position;
        var direction = (lane.End.position - lane.Start.position).normalized;
        var distance = Vector3.Distance(lane.Start.position, lane.End.position);
        position += direction * distance * laneProgress;
        position.z = 0;
        var avatar = Instantiate(this.AvatarPrefab, position, Quaternion.identity);
        avatar.Appearance = appearance;
        avatar.transform.position = position;
        avatar.transform.localScale = Vector3.one * lane.AvatarSize;
    }
}
