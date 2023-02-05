using DG.Tweening;
using UnityEngine;

public class AvatarMovementController : MonoBehaviour
{
    public enum State
    {
        Walking,
        Stoped,
        Rocketing,
    }

    public AvatarAppearanceController AppearanceController;

    private SpawnLane _lane;

    public enum Direction { None = 0, Left = -1, Right = 1 }

    public Direction CurrentDirection { get; private set; } = Direction.Right;

    public float CurrentDistance { get; private set; }

    public MovePreset MovePreset;

    private float _bounceTime;

    private float _swingTime;

    public AvatarAppearance Appearance => this.AppearanceController.Appearance;

    public State CurrentState { get; private set; } = State.Walking;

    private bool _doNotBounceAnymore = false;

    private bool _doNotSwingAnymore = false;

    private float _rocketTime = 0f;

    private float _rocketSpeed = 0f;

    public float RocketAcceleration = 100f;


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
        var distance = this.CurrentDistance;

        float dt = Time.deltaTime * GameManager.Instance.GlobalAvatarTimeScale;
        bool isMoving = (this.CurrentState == State.Walking);

        // Ping pong distance
        distance += move.Speed * dt * (float)this.CurrentDirection * (isMoving ? 1f : 0f);

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
        this._bounceTime += dt * move.BounceSpeed * (this._doNotBounceAnymore ? 0f : 1f);

        if (this._bounceTime > 1f)
        {
            this._bounceTime %= 1f;
            if (!isMoving)
            {
                this._doNotBounceAnymore = true;
            }
        }

        // Adjust swing time
        this._swingTime += dt * move.BounceSpeed * 0.5f * (this._doNotSwingAnymore ? 0f : 1f);

        if (this._swingTime > 0.5f)
        {
            this._swingTime %= 0.5f;
            if (!isMoving)
            {
                this._doNotSwingAnymore = true;
            }
        }

        // Adjust swing (rotation)
        var swingAngle = Mathf.Sin(this._swingTime * Mathf.PI * 2) * move.SwingAngle;
        var swingRotation = Quaternion.Euler(0, 0, swingAngle);
        this.transform.localRotation = swingRotation;


        if (this.CurrentState == State.Rocketing)
        {
            this._rocketTime += dt;

            if (this._rocketTime > 0.5f)
            {
                this._rocketSpeed += this.RocketAcceleration * dt;
                this.transform.position += Vector3.up * this._rocketSpeed;
            }
        }
    }

    // a simple curve that bounces 1 time (half a sine wave)
    private float LerpBounce(float t)
    {
        return Mathf.Abs(Mathf.Sin(t * Mathf.PI * 2));
    }

    public void OnMouseDown()
    {
        if (this.CurrentState == State.Rocketing)
        {
            return;
        }

        bool isCorrect = GameManager.Instance.OnClickAvatar(this);

        if (isCorrect)
        {
            StartRocketing();
        }
        else
        {
            StopWalking();

            // Tint interpolate color to red and back
            var sequence = DOTween.Sequence();
            var red = new Color(0.8f, 0.2f, 0.2f);
            for (int i = 0; i < 5; i++)
            {
                sequence.Append(this.AppearanceController.TintToColor(red, 0.2f));
                sequence.Append(this.AppearanceController.TintToColor(Color.white, 0.2f));
            }
            sequence.OnComplete(() => this.StartWalking());
        }
    }

    public void OnMouseEnter()
    {
        this.AppearanceController.SetHighlight(true);
    }

    public void OnMouseExit()
    {
        this.AppearanceController.SetHighlight(false);
    }

    public void StopWalking()
    {
        this.CurrentState = State.Stoped;
    }

    public void StartRocketing()
    {
        this.CurrentState = State.Rocketing;
    }

    public void StartWalking()
    {
        this.CurrentState = State.Walking;
        this._doNotBounceAnymore = false;
        this._doNotSwingAnymore = false;
    }
}
