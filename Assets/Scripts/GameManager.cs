using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public BodyPartsController BodyPartsController;

    public GuiController GuiController;

    public SpawnerController SpawnerController;

    public float RemainingTime { get; private set; }

    public int NumberOfTargetsToFind = 1;

    public int NumberOfExtras = 2;

    private readonly List<AvatarAppearance> _targetsToFind = new List<AvatarAppearance>();

    public void Start()
    {
        if (Instance != null)
        {
            throw new System.Exception("There can only be one GameManager");
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

        this.GuiController.SetTargets(this._targetsToFind);

        // Debug
        for (int i = 0; i < this.NumberOfExtras; i++)
        {
            var appearance = this.BodyPartsController.GetRandomAppearance();
            this.SpawnerController.SpawnRandom(appearance);
        }
    }

    public void Update()
    {
        this.RemainingTime -= Time.deltaTime;

        if (this.RemainingTime <= 0)
        {
            this.RemainingTime = 0;
        }
        this.GuiController.SetRemainingTime(this.RemainingTime);


        if (this.RemainingTime <= 0)
        {
            this.RemainingTime = 0;
            this.GuiController.ShowGameOver();
            Debug.Log("Game Over");
        }
    }

    internal void OnClickAvatar(AvatarMovementController avatarController)
    {
        if (this._targetsToFind.Contains(avatarController.Appearance))
        {
            this._targetsToFind.Remove(avatarController.Appearance);
            this.GuiController.MarkTargetAsFound(avatarController.Appearance);
            if (this._targetsToFind.Count == 0)
            {
                this.GuiController.ShowGameWon();
            }
        }
    }
}
