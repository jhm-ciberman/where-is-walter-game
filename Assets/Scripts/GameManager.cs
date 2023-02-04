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

    public int NumberOfTargetsToFind { get; set; } = 1;

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

        // Debug
        int extrasCount = 30;
        for (int i = 0; i < extrasCount; i++)
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
}
