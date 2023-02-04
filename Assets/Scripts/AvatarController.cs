using UnityEngine;

public class AvatarController : MonoBehaviour
{

    private AvatarAppearance _appearance;


    public SpriteRenderer BodyRenderer;

    public SpriteRenderer HeadRenderer;

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

    private void UpdateAppearance()
    {
        this.BodyRenderer.sprite = this.Appearance.Body.Sprite;
        this.HeadRenderer.sprite = this.Appearance.Face.Sprite;
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
}
