﻿using System;
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

[RequireComponent(typeof(Collider2D))]
public class Spikes : MonoBehaviour, IPhaseItem
{
    [SerializeField] private SpikeSettings _settings;

    public int Stages { get; private set; }
    private int _currentStage;
    public int CurrentStage
    {
        get { return _currentStage; }
        private set
        {
            if (value >= Stages)
                _currentStage = 0;
            else
                _currentStage = value;

            _settings.SpriteRenderer.sprite = _settings.StageSprites[_currentStage];

            _collider.enabled = value == Stages - 1;
        }
    }

    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;

        Stages = 3;

        CurrentStage = _settings.StartStage;
        if (_settings.RandomStartStage)
            CurrentStage = Random.Range(0, Stages);

        PhaseManager.Instance.RegisterObstacle(this);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (player == null)
            return;
        
        StartCoroutine(KillPlayer(player));
    }

    public void AdvanceStage()
    {
        CurrentStage++;
    }

    private IEnumerator KillPlayer(Player player)
    {
        SoundManager.Instance.Play(Sound.Hurt);
        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.35f);

        GameManager.Instance.State = GameState.GameOver;
    }
}
