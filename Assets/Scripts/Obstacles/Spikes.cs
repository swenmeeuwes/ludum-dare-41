using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpikeSettings
{
    public SpriteRenderer SpriteRenderer;
    public Sprite[] StageSprites;
    public int StartStage;
    public bool RandomStartStage = true;
}

public class Spikes : Obstacle
{
    [SerializeField] private SpikeSettings _settings;

    public int Stages { get; private set; }
    private int _currentStage;
    public int CurrentStage {
        get { return _currentStage; }
        private set
        {
            if (value >= Stages)
                _currentStage = 0;
            else
                _currentStage = value;

            _settings.SpriteRenderer.sprite = _settings.StageSprites[_currentStage];            
        }
    }

    private void Start()
    {
        Stages = 3;

        CurrentStage = _settings.StartStage;
        if (_settings.RandomStartStage)
            CurrentStage = Random.Range(0, Stages);

        PhaseManager.Instance.RegisterObstacle(this);
    }

    public override void AdvanceStage()
    {
        CurrentStage++;
    }
}
