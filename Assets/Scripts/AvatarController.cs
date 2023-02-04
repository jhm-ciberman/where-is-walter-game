using System;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    private AvatarAppearance _appearance;

    public SpriteRenderer BodyRenderer;

    public SpriteRenderer FaceRenderer;

    public SpriteRenderer AccessoryRenderer;

    public SpriteRenderer ClothesRenderer;

    public SpriteRenderer LeftHandRenderer;

    public SpriteRenderer RightHandRenderer;

    public AvatarAppearance Appearance
    {
        get => this._appearance;
        set
        {
            if (this._appearance == value)
                return;
            this._appearance = value;
            this.UpdateAppearance();
        }
    }

    private SpawnLane _lane;

    public enum Direction { None = 0, Left = -1, Right = 1 }

    public Direction CurrentDirection { get; private set; } = Direction.Right;

    public float CurrentDistance { get; private set; }

    public AnimationCurve BounceCurve;

    public MovePreset MovePreset;

    private float _bounceTime;

    private float _swingTime;

    public float DeltaTimeScale = 1.0f;

    private SpriteRenderer[] _renderers;

    public void Awake()
    {
        this._renderers = new SpriteRenderer[]
        {
            this.BodyRenderer,
            this.FaceRenderer,
            this.AccessoryRenderer,
            this.ClothesRenderer,
            this.LeftHandRenderer,
            this.RightHandRenderer,
        };
    }

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
        float y = this.BounceCurve.Evaluate(this._bounceTime);
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

    private void UpdateAppearance()
    {
        this.BodyRenderer.sprite = this.Appearance.Body.Sprite;
        this.FaceRenderer.sprite = this.Appearance.Face.Sprite;
        this.AccessoryRenderer.sprite = this.Appearance.Accessory.Sprite;
        this.ClothesRenderer.sprite = this.Appearance.Clothes.Sprite;
        this.LeftHandRenderer.sprite = this.Appearance.Body.HandSprite;
        this.RightHandRenderer.sprite = this.Appearance.Body.HandSprite;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position + Vector3.up * 0.5f, 0.5f);
    }

    public void SetSortingLayer(int sortingLayerID)
    {
        foreach (var renderer in this._renderers)
        {
            renderer.sortingLayerID = sortingLayerID;
        }
    }

    public void SetSortingOrder(int order)
    {
        foreach (var renderer in this._renderers)
        {
            renderer.sortingOrder = order;
        }
    }
}
