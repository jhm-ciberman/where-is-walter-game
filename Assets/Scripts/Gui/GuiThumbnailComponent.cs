using System;
using UnityEngine;
using UnityEngine.UI;

public class GuiThumbnailComponent : MonoBehaviour
{
    private AvatarAppearance _appearance;

    public Image BodyRenderer;

    public Image FaceRenderer;

    public Image AccessoryRenderer;

    public Image ClothesRenderer;

    public Image LeftHandRenderer;

    public Image RightHandRenderer;

    public Image CrossRenderer;

    public RectTransform AvatarTransform;

    public float SwingSpeed = 0.2f;

    public float SwingAngle = 5f;

    private float _swingTime = 0f;

    private bool _isSwinging = true;

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


    private Image[] _renderers;

    public void Awake()
    {
        this._renderers = new Image[]
        {
            this.BodyRenderer,
            this.FaceRenderer,
            this.AccessoryRenderer,
            this.ClothesRenderer,
            this.LeftHandRenderer,
            this.RightHandRenderer,
        };
    }

    public void Start()
    {
        this.CrossRenderer.gameObject.SetActive(false);
        this._swingTime = UnityEngine.Random.Range(0f, 100f);
        this.SwingSpeed += UnityEngine.Random.Range(-0.1f, 0.1f);
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

    internal void MarkAsFound()
    {
        foreach (var renderer in this._renderers)
        {
            renderer.color = Color.gray;
        }

        this.CrossRenderer.gameObject.SetActive(true);
        this._isSwinging = false;
    }

    public void Update()
    {
        if (!this._isSwinging)
            return;
        this._swingTime += Time.deltaTime * this.SwingSpeed;
        this.AvatarTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(this._swingTime) * this.SwingAngle);
    }
}
