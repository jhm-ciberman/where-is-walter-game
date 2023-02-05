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

    private void UpdateAppearance()
    {
        this.BodyRenderer.sprite = this.Appearance.Body.Sprite;
        this.FaceRenderer.sprite = this.Appearance.Face.Sprite;
        this.AccessoryRenderer.sprite = this.Appearance.Accessory.Sprite;
        this.ClothesRenderer.sprite = this.Appearance.Clothes.Sprite;
        this.LeftHandRenderer.sprite = this.Appearance.Body.HandSprite;
        this.RightHandRenderer.sprite = this.Appearance.Body.HandSprite;
    }

    public void SetColor(Color color)
    {
        foreach (var renderer in this._renderers)
        {
            renderer.color = color;
        }
    }
}
