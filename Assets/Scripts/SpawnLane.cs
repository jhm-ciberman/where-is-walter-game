using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnLane : MonoBehaviour
{
    public Transform Start;

    public Transform End;

    public float AvatarSize;

    [Range(0, 1)]
    public float PreviewAvatarProgress = 0.5f;

    public Vector3 ProgressToPosition(float laneProgress)
    {
        var position = this.Start.position;
        var direction = (this.End.position - this.Start.position).normalized;
        var distance = Vector3.Distance(this.Start.position, this.End.position);
        position += direction * distance * laneProgress;
        return position;
    }

    public Vector3 DistanceToPosition(float distance)
    {
        var position = this.Start.position;
        var direction = (this.End.position - this.Start.position).normalized;
        position += direction * distance;
        return position;
    }


    public float Distance => Vector3.Distance(this.Start.position, this.End.position);

    public float ProgressToDistance(float progress)
    {
        return this.Distance * progress;
    }

    protected void OnDrawGizmos()
    {
        if (this.Start == null || this.End == null)
        {
            return;
        }

        //this.PingPongPreviewAvatar();

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.Start.position, 0.3f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.End.position, 0.3f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(this.Start.position, this.End.position);

        // Draw preview avatar
        var radius = this.AvatarSize * 0.5f;
        var position = this.ProgressToPosition(this.PreviewAvatarProgress);
        position.y += radius;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, radius);

        string name = this.gameObject.name ?? "Unnamed";

        var labelPosition = this.Start.position + (this.End.position - this.Start.position) * 0.5f;
        Handles.Label(labelPosition, name);
    }
}
