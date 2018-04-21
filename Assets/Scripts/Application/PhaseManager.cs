using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PhaseSettings
{
    public int PlayerStaminaPerPhase = 2;
}

public enum Phase
{
    Player,
    Enemy
}

public class PhaseManager : MonoSingletonEventDispatcher<PhaseManager>
{
    public static readonly string PlayerStaminaChanged = "PhaseManager.PlayerStaminaChanged";
    public static readonly string CurrentStaminaCostChanged = "PhaseManager.CurrentStaminaCostChanged";

    [SerializeField] private ExecuteHolder _executeHolder;
    [SerializeField] private PhaseSettings _settings;

    private Phase _currentPhase;
    public Phase CurrentPhase {
        get { return _currentPhase; }
        set
        {
            if (value == _currentPhase)
                return;

            switch (value)
            {
                case Phase.Player:
                    PlayerStamina = _settings.PlayerStaminaPerPhase;
                    break;
            }

            _currentPhase = value;
        }
    }

    private int _playerStamina;
    public int PlayerStamina {
        get { return _playerStamina; }
        set
        {
            _playerStamina = value;
            Dispatch(new EventObject
            {
                Sender = this,
                Type = PlayerStaminaChanged,
                Data = value
            });
        }
    }

    private int _currentStaminaCost;
    public int CurrentStaminaCost {
        get { return _currentStaminaCost; }
        set
        {
            _currentStaminaCost = value;
            Dispatch(new EventObject
            {
                Sender = this,
                Type = CurrentStaminaCostChanged,
                Data = value
            });
        }
    }

    protected override void Awake()
    {
        base.Awake();

        DefineSingleton(this);
    }

    private void Start()
    {
        CurrentPhase = Phase.Player;
        PlayerStamina = _settings.PlayerStaminaPerPhase;
    }

    public void ExecuteCards()
    {
        var cards = _executeHolder.Cards;
        CardManager.Instance.ExecuteCards(cards);
        //CurrentPhase = Phase.Enemy;
    }
}
