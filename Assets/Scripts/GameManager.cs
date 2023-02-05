using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public BodyPartsController BodyPartsController;

    public GuiController GuiController;

    public SpawnerController SpawnerController;

    public float RemainingTime { get; private set; }

    public float GlobalAvatarTimeScale = 0.35f;

    public int NumberOfTargetsToFind = 1;

    public int NumberOfExtras = 2;

    public int MaxNumberOfAttempts = 3;

    public string NextSceneName = "MenuArbol";

    private readonly List<AvatarAppearance> _targetsToFind = new List<AvatarAppearance>();

    private int _attempts = 0;

    private enum GameState
    {
        Playing,
        GameOver,
        GameWon,
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
            this.SpawnerController.SpawnRandom(appearance);
        }

        this.GuiController.SetTargetAvatars(this._targetsToFind);

        // Debug
        for (int i = 0; i < this.NumberOfExtras; i++)
        {
            var appearance = this.BodyPartsController.GetRandomAppearance();
            this.SpawnerController.SpawnRandom(appearance);
        }

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

    }

    public bool OnClickAvatar(AvatarMovementController avatarController)
    {
        if (this._targetsToFind.Contains(avatarController.Appearance))
        {
            this._targetsToFind.Remove(avatarController.Appearance);
            this.GuiController.MarkTargetAvatarAsFound(avatarController.Appearance);
            if (this._targetsToFind.Count == 0)
            {
                this.WinLevel();
            }

            return true;
        }
        else if (this._attempts < this.MaxNumberOfAttempts)
        {
            this._attempts++;

            this.GuiController.SetRemainingAttempts(this.MaxNumberOfAttempts - this._attempts);
            if (this._attempts >= this.MaxNumberOfAttempts)
            {
                this.LooseLevel();
            }
        }

        return false;
    }

    private void WinLevel()
    {
        this._gameState = GameState.GameWon;
        this.GuiController.ShowGameWon();
        // Load next scene
        DOVirtual.DelayedCall(3f, () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(this.NextSceneName);
        });
    }

    private void LooseLevel()
    {
        this._gameState = GameState.GameOver;
        this.GuiController.ShowGameOver();

        // Load next scene
        DOVirtual.DelayedCall(3f, () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(this.NextSceneName);
        });
    }
}
