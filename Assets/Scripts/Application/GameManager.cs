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
    public GameState State {
        get { return _state; }
        set
        {
            if (_state == value)
                return;

            HandleStateChanged(_state, value);
            _state = value;
        }
    }

    private void Awake()
    {
        DefineSingleton(this);
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
