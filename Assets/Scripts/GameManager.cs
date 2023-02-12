using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public BodyPartsController BodyPartsController;

    public GuiController GuiController;

    public SpawnerController SpawnerController;

    public CameraController CameraController;

    public float RemainingTime { get; private set; }

    public float GlobalAvatarTimeScale = 0.35f;

    public int NumberOfTargetsToFind = 1;

    public int NumberOfExtras = 2;

    public int MaxNumberOfAttempts = 3;

    private readonly List<AvatarAppearance> _targetsToFind = new List<AvatarAppearance>();

    private readonly List<AvatarAppearance> _initialTargetsToFind = new List<AvatarAppearance>();

    private int _attempts = 0;

    private enum GameState
    {
        Playing,
        GameOver,
        GameWon,
        Cutscene,
    }

    private GameState _gameState = GameState.Playing;

    public void Start()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this.gameObject);
        }

        Instance = this;

        if (this.BodyPartsController == null)
        {
            throw new System.Exception("BodyPartsController is not set");
        }

        if (this.GuiController == null)
        {
            throw new System.Exception("GuiController is not set");
        }

        this.RemainingTime = 60;

        this.BodyPartsController.ResetExcludedAppearances();

        this._targetsToFind.Clear();

        // Init targets to find
        for (int i = 0; i < this.NumberOfTargetsToFind; i++)
        {
            var appearance = this.BodyPartsController.GetRandomAppearance();
            this.BodyPartsController.ExcludeAppearance(appearance);
            this._targetsToFind.Add(appearance);
            this._initialTargetsToFind.Add(appearance);
            this.SpawnerController.SpawnRandom(appearance);
        }

        this.GuiController.SetTargetAvatars(this._targetsToFind);

        // Debug
        for (int i = 0; i < this.NumberOfExtras; i++)
        {
            var appearance = this.BodyPartsController.GetRandomAppearance();
            this.SpawnerController.SpawnRandom(appearance);
        }

        this.GuiController.gameObject.SetActive(true);
        this.GuiController.SetRemainingAttempts(this.MaxNumberOfAttempts);
        this.GuiController.SetRemainingTime(this.RemainingTime);
    }

    public void Update()
    {
        if (this._gameState == GameState.Playing)
        {

            this.RemainingTime -= Time.deltaTime;

            if (this.RemainingTime <= 0)
            {
                this.RemainingTime = 0;
            }
            this.GuiController.SetRemainingTime(this.RemainingTime);

            if (this.RemainingTime <= 0)
            {
                this.LooseLevel();
            }
        }

        this.GuiController.SetCutsceneMode(this._gameState != GameState.Playing);
    }

    public enum ClickResult
    {
        None,
        Correct,
        Incorrect,
    }

    public ClickResult OnClickAvatar(AvatarMovementController avatarController)
    {
        if (this._gameState != GameState.Playing)
        {
            return ClickResult.None;
        }

        if (this._targetsToFind.Contains(avatarController.Appearance))
        {
            this.ClickAvatarCorrect(avatarController);

            return ClickResult.Correct;
        }

        this.ClickAvatarIncorrect(avatarController);

        return ClickResult.Incorrect;
    }

    private void ClickAvatarCorrect(AvatarMovementController avatarController)
    {
        this._targetsToFind.Remove(avatarController.Appearance);
        this.GuiController.MarkTargetAvatarAsFound(avatarController.Appearance);

        this._gameState = GameState.Cutscene;
        avatarController.PlayCorrectAnimation();
        var headPosition = avatarController.GetHeadPosition();
        this.CameraController.FocusOn(headPosition, 0f, 3f);
        DOVirtual.DelayedCall(3f, () => this.ReturnToPlayFromCutscene());

        if (this._targetsToFind.Count == 0)
        {
            this.WinLevel();
        }
    }

    private void ReturnToPlayFromCutscene()
    {
        if (this._gameState == GameState.Cutscene)
        {
            this._gameState = GameState.Playing;
        }
    }

    private void ClickAvatarIncorrect(AvatarMovementController avatarController)
    {
        if (this._attempts >= this.MaxNumberOfAttempts) return;

        this._attempts++;

        this._gameState = GameState.Cutscene;
        avatarController.PlayIncorrectAnimation();
        var headPosition = avatarController.GetHeadPosition();
        this.CameraController.FocusOn(headPosition, 0f, 2f);
        DOVirtual.DelayedCall(2f, () => this.ReturnToPlayFromCutscene());

        this.GuiController.SetRemainingAttempts(this.MaxNumberOfAttempts - this._attempts);
        if (this._attempts >= this.MaxNumberOfAttempts)
        {
            this.LooseLevel(3f);
        }
    }

    private void WinLevel()
    {
        if (this._gameState == GameState.GameWon)
        {
            return;
        }
        this._gameState = GameState.GameWon;

        var level = LevelManager.Instance.GetCurrentLevel();
        LevelManager.Instance.NotifyLevelCompleted(level);
        LevelManager.Instance.SaveAvatarsForLevel(level, this._initialTargetsToFind);

        DOVirtual.DelayedCall(4f, () => this.GuiController.ShowGameWon());
        DOVirtual.DelayedCall(7f, () => SceneManager.LoadScene("TreeMenu"));
    }

    private void LooseLevel(float delay = 0f)
    {
        if (this._gameState == GameState.GameOver)
        {
            return;
        }
        this._gameState = GameState.GameOver;

        DOVirtual.DelayedCall(delay + 1f, () => this.GuiController.ShowGameOver());
        DOVirtual.DelayedCall(delay + 4f, () => SceneManager.LoadScene("TreeMenu"));
    }
}
