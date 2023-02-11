using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GuiThumbnailComponent : MonoBehaviour
{

    public RectTransform AvatarTransform;

    public Image CrossRenderer;

    public GuiAvatar AvatarController;

    public RectTransform TargetObjectParentTransform;

    public GuiSwingController SwingController;


    private RectTransform _targetObjectGameObject;
    public RectTransform TargetObjectGameObject
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

        this.CrossRenderer.transform.localScale = Vector3.zero;
        DOTween.To(
            () => this.CrossRenderer.transform.localScale,
            x => this.CrossRenderer.transform.localScale = x,
            Vector3.one,
            0.5f
        ).SetEase(Ease.OutBack);
    }

    private void InstantiateTargetObject(RectTransform targetObject)
    {
        if (targetObject == null)
            return;

        var go = Instantiate(targetObject, this.TargetObjectParentTransform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
    }

    private void DeinstantiateTargetObject(RectTransform targetObject)
    {
        if (targetObject == null)
            return;

        Destroy(targetObject);
    }
}
