using System;
using Hellmade.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    public Action SceneChanged;

    public Action TransitionEnded;

    public Image Panel1;

    public Image Panel2;

    public float TransitionTime = 1f;

    private float _progress = 0;

    private bool _isIntro = true; // true = intro, false = outro

    public string NextScene;

    public AudioClip TransitionSound;

    public void Start()
    {
        if (TransitionManager.Instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        TransitionManager.Instance = this;

        DontDestroyOnLoad(this.gameObject);

        this.Panel1.enabled = true;
        this.Panel2.enabled = true;

        EazySoundManager.PlayMusic(this.TransitionSound, 0.5f, false, persist: true);
    }

    private void SetCutoff(Image image, float cutoff)
    {
        image.material.SetFloat("_Cutoff", cutoff);
    }

    public void SetMirror(Image image, bool mirrorX, bool mirrorY)
    {
        Vector3 scale = image.transform.localScale;
        scale.x = mirrorX ? -1 : 1;
        scale.y = mirrorY ? -1 : 1;
        image.transform.localScale = scale;
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(this.NextScene);
        this.SceneChanged?.Invoke();
    }

    private void UpdateProgress()
    {
        float delta = Time.deltaTime / this.TransitionTime;
        if (this._isIntro)
        {
            this._progress += delta;
            if (this._progress >= 1)
            {
                this._progress = 1;
                this._isIntro = false;
                this.ChangeScene();
            }
        }
        else
        {
            this._progress -= delta;
            if (this._progress <= 0)
            {
                this._progress = 0;
                this._isIntro = true;
                this.TransitionEnded?.Invoke();
                MonoBehaviour.Destroy(this.gameObject);
            }
        }
    }

    public void Update()
    {
        this.UpdateProgress();
        var p = 1f - this._progress;
        this.SetCutoff(this.Panel1, p);
        this.SetCutoff(this.Panel2, (p * p) - 0.1f);
        this.SetMirror(this.Panel1, this._isIntro, this._isIntro);
        this.SetMirror(this.Panel2, this._isIntro, this._isIntro);
    }

    public void TransitionToScene(string sceneName)
    {
        this.NextScene = sceneName;
        this._isIntro = true;
        this._progress = 0;
    }
}
