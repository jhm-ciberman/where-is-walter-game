using System;
using DG.Tweening;
using UnityEngine;

public class AvatarAppearanceController : MonoBehaviour
{
    private AvatarAppearance _appearance;

    public SpriteRenderer BodyRenderer;

    public SpriteRenderer FaceRenderer;

    public SpriteRenderer AccessoryRenderer;

    public SpriteRenderer ClothesRenderer;

    public SpriteRenderer LeftHandRenderer;

    public SpriteRenderer RightHandRenderer;

    public Material HighlightMaterial;

    public Material DefaultMaterial;

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

    public void SetTint(Color color)
    {
        foreach (var renderer in this._renderers)
        {
            renderer.color = color;
        }
    }

    public Color GetTintColor()
    {
        return this.BodyRenderer.color;
    }

    internal Tween TintToColor(Color color, float duration = 0.5f)
    {
        return DOTween.To(
            () => this.GetTintColor(),
            x => this.SetTint(x),
            color,
            duration);
    }

    internal void SetHighlight(bool isHighlighted)
    {
        foreach (var renderer in this._renderers)
        {
            renderer.material = isHighlighted ? this.HighlightMaterial : this.DefaultMaterial;
        }
    }

    internal Vector3 GetHeadPosition()
    {
        return this.transform.position + Vector3.up * this.transform.localScale.y * 0.5f;
    }
}
