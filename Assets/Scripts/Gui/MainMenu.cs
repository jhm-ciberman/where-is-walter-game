using Hellmade.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip MusicClip;

    public BodyPartsController BodyPartsController;

    public GuiAvatar[] Avatars;

    public float AvatarChangeInterval = 1f;

    private float _avatarChangeCountdown = 0f;

    public void Start()
    {
        EazySoundManager.PlayMusic(this.MusicClip, 1.0f, true, false);

        this._avatarChangeCountdown = this.AvatarChangeInterval;
        this.ChangeAppearances();
    }

    public void OnStartButtonClicked()
    {
        LevelManager.Instance.TransitionToScene("TreeMenu");
    }

    public void OnCreditsButtonClicked()
    {
        LevelManager.Instance.TransitionToScene("Credits");
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    public void Update()
    {
        this._avatarChangeCountdown -= Time.deltaTime;
        if (this._avatarChangeCountdown <= 0f)
        {
            this._avatarChangeCountdown = this.AvatarChangeInterval;

            this.ChangeAppearances();
        }
    }

    private void ChangeAppearances()
    {
        foreach (var avatar in this.Avatars)
        {
            avatar.Appearance = this.BodyPartsController.GetRandomAppearance();
        }
    }
}
