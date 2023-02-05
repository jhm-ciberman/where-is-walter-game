using System;
using UnityEngine;
using UnityEngine.UI;

public class GuiThumbnailComponent : MonoBehaviour
{


    public RectTransform AvatarTransform;

    public float SwingSpeed = 0.2f;

    public float SwingAngle = 5f;

    public float BounceSpeed = 0.2f;

    public float BounceDelta = 0.1f;

    private float _swingTime = 0f;

    private float _bounceTime = 0f;

    private bool _isStatic = false;

    private Vector3 _initialAvatarPosition;

    public Image CrossRenderer;

    public GuiAvatar AvatarController;

    public RectTransform TargetObjectParentTransform;


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
        this._swingTime = UnityEngine.Random.Range(0f, 100f);
        this.SwingSpeed += UnityEngine.Random.Range(-0.1f, 0.1f);
        this._initialAvatarPosition = this.AvatarTransform.localPosition;
    }



    internal void MarkAsFound()
    {
        this.AvatarController.SetColor(Color.gray);
        this.CrossRenderer.gameObject.SetActive(true);
        this._isStatic = true;
    }

    public void Update()
    {
        if (this._isStatic)
            return;
        this._swingTime += Time.deltaTime * this.SwingSpeed;
        this._bounceTime += Time.deltaTime * this.BounceSpeed;
        this.AvatarTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(this._swingTime) * this.SwingAngle);
        this.AvatarTransform.localPosition = this._initialAvatarPosition + new Vector3(0f, Mathf.Sin(this._bounceTime) * this.BounceDelta, 0f);
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
