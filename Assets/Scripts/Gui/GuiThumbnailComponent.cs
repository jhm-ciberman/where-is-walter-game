using System;
using UnityEngine;
using UnityEngine.UI;

public class GuiThumbnailComponent : MonoBehaviour
{

    public RectTransform AvatarTransform;

    public Image CrossRenderer;

    public GuiAvatar AvatarController;

    public RectTransform TargetObjectParentTransform;

    public GuiSwingController SwingController;


    private GameObject _targetObjectGameObject;
    public GameObject TargetObjectGameObject
    {
        get => this._targetObjectGameObject;
        set
        {
            var old = this._targetObjectGameObject;
            if (old == value) return;

            DeinstantiateTargetObject(old);
            this._targetObjectGameObject = value;
            bool showAvatar = value == null;
            this.AvatarTransform.gameObject.SetActive(showAvatar);
            InstantiateTargetObject(value);
        }
    }


    public AvatarAppearance Appearance
    {
        get => this.AvatarController.Appearance;
        set => this.AvatarController.Appearance = value;
    }

    public void Start()
    {
        this.CrossRenderer.gameObject.SetActive(false);
    }

    internal void MarkAsFound()
    {
        this.AvatarController.SetColor(Color.gray);
        this.CrossRenderer.gameObject.SetActive(true);
        this.SwingController.IsStatic = true;
    }

    private void InstantiateTargetObject(GameObject targetObject)
    {
        if (targetObject == null)
            return;

        var go = Instantiate(targetObject, this.TargetObjectParentTransform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
    }

    private void DeinstantiateTargetObject(GameObject targetObject)
    {
        if (targetObject == null)
            return;

        Destroy(targetObject);
    }
}
