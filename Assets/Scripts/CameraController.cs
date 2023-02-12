using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera Camera;

    public Vector2 WiggleDelta = new Vector2(2.011f, 0.11f);

    public Vector2 WiggleSpeed = new Vector2(0.67f, 1.73f);

    public Vector2 MicroWiggleDelta = new Vector2(0.6f, 0.2f);
    public Vector2 MicroWiggleSpeed = new Vector2(0.9f, 0.7f);

    public SpriteRenderer BackgroundBounds;

    private Vector2 _wiggleTime = Vector2.zero;

    private Vector2 _microWiggleTime = Vector2.zero;

    public float FocusTransitionDuration = 0.5f;

    private Vector3 _focusTarget;

    private bool _isFocusing = false;

    private float _idleSize = 0;

    private float _focusSize = 0;

    private float _focusResetTimer = float.PositiveInfinity;

    private float _focusProgress = 0;  // 0..1

    public float DefaultFocusSize = 3;

    private Rect _backgroundBounds;

    public float ExtraPadding = 0.2f;

    public void FocusOn(Vector3 target, float size = 0f, float resetAfter = float.PositiveInfinity)
    {
        this._isFocusing = true;
        this._focusTarget = new Vector3(target.x, target.y, this.transform.position.z);
        this._focusSize = size > 0 ? size : this.DefaultFocusSize;
        this._focusProgress = 0;
        this._focusResetTimer = resetAfter;
    }

    public void Start()
    {
        if (this.Camera == null)
            this.Camera = Camera.main;

        this._idleSize = this.Camera.orthographicSize;
        this._focusTarget = this.transform.position;

        this._backgroundBounds = this.ComputeBackgroundBounds();
    }

    private Rect ComputeBackgroundBounds()
    {
        var bounds = this.BackgroundBounds.bounds;
        var size = bounds.size;
        var center = bounds.center;
        return new Rect(center.x - size.x / 2, center.y - size.y / 2, size.x, size.y);
    }

    private Vector3 ClampToBackgroundBounds(Vector3 position)
    {
        var bounds = this._backgroundBounds;
        var padding = Vector3.one * (this._focusSize + this.ExtraPadding);
        var x = Mathf.Clamp(position.x, bounds.xMin + padding.x, bounds.xMax - padding.x);
        var y = Mathf.Clamp(position.y, bounds.yMin + padding.y, bounds.yMax - padding.y);
        return new Vector3(x, y, position.z);
    }


    private Vector3 ComputeIdlePosition()
    {
        this._wiggleTime.x += Time.deltaTime * this.WiggleSpeed.x;
        this._wiggleTime.y += Time.deltaTime * this.WiggleSpeed.y;

        this._microWiggleTime.x += Time.deltaTime * this.MicroWiggleSpeed.x;
        this._microWiggleTime.y += Time.deltaTime * this.MicroWiggleSpeed.y;

        var x = Mathf.Sin(this._wiggleTime.x) * this.WiggleDelta.x;
        var y = Mathf.Sin(this._wiggleTime.y) * this.WiggleDelta.y;

        var mx = Mathf.Sin(this._microWiggleTime.x) * this.MicroWiggleDelta.x * 0.1f;
        var my = Mathf.Sin(this._microWiggleTime.y) * this.MicroWiggleDelta.y * 0.1f;

        return new Vector3(x + mx, y + my, this.transform.localPosition.z);
    }

    public void Update()
    {
        var t = this._focusProgress;
        var idlePosition = this.ComputeIdlePosition();
        var focusPosition = this._focusTarget;
        var position = Vector3.Lerp(idlePosition, focusPosition, t * t);
        position = this.ClampToBackgroundBounds(position);

        var idleSize = this._idleSize;
        var focusSize = this._focusSize;
        var size = Mathf.SmoothStep(idleSize, focusSize, t);

        this.transform.localPosition = position;
        this.Camera.orthographicSize = size;

        if (this._isFocusing)
        {
            this._focusProgress += Time.deltaTime / this.FocusTransitionDuration;
            if (this._focusProgress >= 1)
            {
                this._focusProgress = 1;
            }

            this._focusResetTimer -= Time.deltaTime;
            if (this._focusResetTimer <= 0)
            {
                this._focusResetTimer = float.PositiveInfinity;
                this._isFocusing = false;
            }
        }
        else
        {

            this._focusProgress -= Time.deltaTime / this.FocusTransitionDuration;
            if (this._focusProgress <= 0)
            {
                this._focusProgress = 0;
            }
        }
    }
}
