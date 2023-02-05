using UnityEngine;

public class GuiSwingController : MonoBehaviour
{
    public RectTransform AvatarTransform;

    public float SwingSpeed = 0.2f;

    public float SwingAngle = 5f;

    public float BounceSpeed = 0.2f;

    public float BounceDelta = 0.1f;

    private float _swingTime = 0f;

    private float _bounceTime = 0f;

    public bool IsStatic = false;

    private Vector3 _initialAvatarPosition;


    public void Start()
    {
        this._swingTime = UnityEngine.Random.Range(0f, 100f);
        this.SwingSpeed += UnityEngine.Random.Range(-0.1f, 0.1f);
        this._initialAvatarPosition = this.AvatarTransform.localPosition;
    }

    public void Update()
    {
        if (this.IsStatic)
            return;
        this._swingTime += Time.deltaTime * this.SwingSpeed;
        this._bounceTime += Time.deltaTime * this.BounceSpeed;
        this.AvatarTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(this._swingTime) * this.SwingAngle);
        this.AvatarTransform.localPosition = this._initialAvatarPosition + new Vector3(0f, Mathf.Sin(this._bounceTime) * this.BounceDelta, 0f);
    }
}
