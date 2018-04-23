using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Idle,
    Playing,
    GameOver
}

public class GameManager : MonoSingleton<GameManager>
{
    private GameState _state;
    public GameState State
    {
        get { return _state; }
        set
        {
            if (_state == value)
                return;

            HandleStateChanged(_state, value);
            _state = value;
        }
    }

    private bool _isReloading;

    private Player _player;
    private EndMarker _endMarker;

    private void Awake()
    {
        DefineSingleton(this);
    }
    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _endMarker = FindObjectOfType<EndMarker>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            _isReloading = true;
            SceneLoader.Instance.ReloadCurrentSceneAsync();
        }
    }

    public Vector2Int ComputeDifferenceBetweenPlayerAndGoal()
    {
        var diff = _endMarker.transform.position - _player.transform.position;
        var roundedDiff = Vector3Int.RoundToInt(diff);
        return new Vector2Int(roundedDiff.x, roundedDiff.y);
    }

    private void HandleStateChanged(GameState previousState, GameState newState)
    {
        switch (newState)
        {
            case GameState.Idle:
                break;
            case GameState.Playing:
                break;
            case GameState.GameOver:
                ScreenTransitionHandler.Instance.IsDieing();
                SceneLoader.Instance.ReloadCurrentSceneAsync();
                break;
        }
    }
}
