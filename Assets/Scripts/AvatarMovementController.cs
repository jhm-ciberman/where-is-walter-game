using System;
using DG.Tweening;
using Hellmade.Sound;
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

    public GameObject SmokeParticlePrefab;

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

    public GameObject ExplosionParticlePrefab;

    public AudioClip ExplosionSound;

    public AudioClip RocketSound;

    private float _flipCountdown = 0f;

    private System.Random _random;


    public void InitializeRandom(System.Random random)
    {
        this._random = random;
        this._bounceTime = (float)random.NextDouble();
        this._swingTime = this._bounceTime;
        this._flipCountdown = this.NextFlipCountdown();
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

            if (this._rocketTime > 0.4f)
            {
                this._rocketSpeed += this.RocketAcceleration * dt;
                this.transform.position += Vector3.up * this._rocketSpeed;
            }
        }

        if (this.CurrentState == State.Walking)
        {
            this._flipCountdown -= dt;
            if (this._flipCountdown < 0f)
            {
                this._flipCountdown = this.NextFlipCountdown();
                this.CurrentDirection = (Direction)(-(int)this.CurrentDirection);
            }
        }
    }

    private float NextFlipCountdown()
    {
        return (float)this._random.NextDouble() * 6f + 3f;
    }

    // a simple curve that bounces 1 time (half a sine wave)
    private float LerpBounce(float t)
    {
        return Mathf.Abs(Mathf.Sin(t * Mathf.PI * 2));
    }

    public void OnMouseDown()
    {
        if (this.CurrentState == State.Walking)
        {
            GameManager.Instance.OnClickAvatar(this);
        }

    }

    public void PlayIncorrectAnimation()
    {
        this.CurrentState = State.Stoped;

        // Tint interpolate color to red and back
        var sequence = DOTween.Sequence();
        var red = new Color(0.8f, 0.2f, 0.2f);
        this.AppearanceController.SetTint(red);
        for (int i = 0; i < 4; i++)
        {
            sequence.Append(this.AppearanceController.TintToColor(Color.white, 0.15f));
            sequence.Append(this.AppearanceController.TintToColor(red, 0.15f));
        }
        sequence.OnComplete(() => this.Explode());
    }

    public void OnMouseEnter()
    {
        this.AppearanceController.SetHighlight(true);
    }

    public void OnMouseExit()
    {
        this.AppearanceController.SetHighlight(false);
    }

    public void PlayCorrectAnimation()
    {
        if (this.CurrentState == State.Rocketing) return;

        this.CurrentState = State.Rocketing;

        Quaternion smokeRotation = Quaternion.Euler(90, 0, 0);
        Instantiate(this.SmokeParticlePrefab, this.transform.position, smokeRotation, this.transform);

        this.AppearanceController.SetHighlight(false);
        this.AppearanceController.SetSortingOrder(1000);

        DOVirtual.DelayedCall(0.2f, () => EazySoundManager.PlaySound(this.RocketSound, volume: 0.8f));
    }

    public void StartWalking()
    {
        this.CurrentState = State.Walking;
        this._doNotBounceAnymore = false;
        this._doNotSwingAnymore = false;
    }

    public Vector3 GetHeadPosition()
    {
        return this.AppearanceController.GetHeadPosition();
    }

    public void Explode()
    {
        var headPosition = this.GetHeadPosition();
        GameObject.Instantiate(this.ExplosionParticlePrefab, headPosition, Quaternion.identity);
        GameObject.Destroy(this.gameObject, .25f);

        var pos = this.transform.localPosition;
        this.transform.localPosition = pos + Vector3.forward * 0.1f; // move forward a bit so its hidden behind the explode particle

        EazySoundManager.PlaySound(this.ExplosionSound, volume: 0.8f);
    }
}
