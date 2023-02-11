using System;
using UnityEngine;
using UnityEngine.UI;

public class GuiAvatar : MonoBehaviour
{
    private AvatarAppearance _appearance;

    public Image BodyRenderer;

    public Image FaceRenderer;

    public Image AccessoryRenderer;

    public Image ClothesRenderer;

    public Image LeftHandRenderer;

    public Image RightHandRenderer;


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


    private Image[] _renderers = Array.Empty<Image>();

    private Sprite _initialBodySprite;

    private Sprite _initialHandSprite;

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
        this._initialBodySprite = this.BodyRenderer.sprite;
        this._initialHandSprite = this.LeftHandRenderer.sprite;

        this.UpdateAppearance();
    }

    private void SetSprite(Image renderer, Sprite sprite, Sprite defaultSprite = null)
    {
        renderer.sprite = sprite ?? defaultSprite;
        renderer.gameObject.SetActive(renderer.sprite != null);
    }

    private void UpdateAppearance()
    {
        if (this._appearance == null)
        {
            this.SetSprite(this.BodyRenderer, null, this._initialBodySprite);
            this.SetSprite(this.FaceRenderer, null);
            this.SetSprite(this.AccessoryRenderer, null);
            this.SetSprite(this.ClothesRenderer, null);
            this.SetSprite(this.LeftHandRenderer, null, this._initialHandSprite);
            this.SetSprite(this.RightHandRenderer, null, this._initialHandSprite);
        }
        else
        {
            this.SetSprite(this.BodyRenderer, this._appearance.Body.Sprite, this._initialBodySprite);
            this.SetSprite(this.FaceRenderer, this._appearance.Face.Sprite);
            this.SetSprite(this.AccessoryRenderer, this._appearance.Accessory.Sprite);
            this.SetSprite(this.ClothesRenderer, this._appearance.Clothes.Sprite);
            this.SetSprite(this.LeftHandRenderer, this._appearance.Body.HandSprite, this._initialHandSprite);
            this.SetSprite(this.RightHandRenderer, this._appearance.Body.HandSprite, this._initialHandSprite);
        }
    }

    public void SetColor(Color color)
    {
        foreach (var renderer in this._renderers)
        {
            renderer.color = color;
        }
    }
}
