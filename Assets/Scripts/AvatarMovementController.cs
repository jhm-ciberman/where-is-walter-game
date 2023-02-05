using UnityEngine;

public class AvatarMovementController : MonoBehaviour
{
    //public enum State
    //{
    //    Walking,
    //    Waiting,
    //    Stoped,
    //}

    public AvatarAppearanceController AppearanceController;

    private SpawnLane _lane;

    public enum Direction { None = 0, Left = -1, Right = 1 }

    public Direction CurrentDirection { get; private set; } = Direction.Right;

    public float CurrentDistance { get; private set; }

    public MovePreset MovePreset;

    private float _bounceTime;

    private float _swingTime;

    public float DeltaTimeScale = 0.5f;

    public AvatarAppearance Appearance => this.AppearanceController.Appearance;

    //public State CurrentState { get; private set; } = State.Walking;



    public void InitializeRandom(System.Random random)
    {
        this._bounceTime = (float)random.NextDouble();
        this._swingTime = this._bounceTime;
    }

    public void SetLane(SpawnLane lane, float progress)
    {
        this._lane = lane;
        this.CurrentDistance = lane.ProgressToDistance(progress);
    }

    public void Update()
    {
        var lane = this._lane;
        if (lane == null)
        {
            throw new System.Exception("Lane is not set");
        }

        var move = this.MovePreset;
        var totalDistance = lane.Distance;

        // Ping pong distance
        var distance = this.CurrentDistance;
        distance += move.Speed * Time.deltaTime * (float)this.CurrentDirection;

        if (distance < 0)
        {
            distance = 0;
            this.CurrentDirection = Direction.Right;
        }
        else if (distance > totalDistance)
        {
            distance = totalDistance;
            this.CurrentDirection = Direction.Left;
        }

        this.CurrentDistance = distance;

        float dt = Time.deltaTime * this.DeltaTimeScale;

        // Adjust scale
        var scale = this.transform.localScale;
        scale.x = (float)this.CurrentDirection * Mathf.Abs(scale.x);
        this.transform.localScale = scale;

        // Adjust position
        var basePosition = lane.DistanceToPosition(distance);
        float y = this.LerpBounce(this._bounceTime);
        var deltaY = y * move.BounceHeight * lane.AvatarSize;
        this.transform.position = basePosition + Vector3.up * deltaY;

        // Adjust bounce time
        this._bounceTime += dt * move.BounceSpeed;
        this._bounceTime %= 1;

        // Adjust swing time
        this._swingTime += dt * move.BounceSpeed / 2;
        this._swingTime %= 1;

        // Adjust swing (rotation)
        var swingAngle = Mathf.Sin(this._swingTime * Mathf.PI * 2) * move.SwingAngle;
        var swingRotation = Quaternion.Euler(0, 0, swingAngle);
        this.transform.localRotation = swingRotation;

    }

    // a simple curve that bounces 1 time (half a sine wave)
    private float LerpBounce(float t)
    {
        return Mathf.Abs(Mathf.Sin(t * Mathf.PI * 2));
    }

    public void OnMouseDown()
    {
        GameManager.Instance.OnClickAvatar(this);
    }
}
