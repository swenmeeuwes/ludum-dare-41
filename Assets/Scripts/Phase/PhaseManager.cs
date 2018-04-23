using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PhaseSettings
{
    public int PlayerStaminaPerPhase = 2;
}

public enum Phase
{
    Player,
    Enemy,
    Obstacles
}

public class PhaseManager : MonoSingletonEventDispatcher<PhaseManager>
{
    public static readonly string PlayerStaminaChanged = "PhaseManager.PlayerStaminaChanged";
    public static readonly string CurrentStaminaCostChanged = "PhaseManager.CurrentStaminaCostChanged";
    public static readonly string PhaseChanged = "PhaseManager.PhaseChanged";

    [SerializeField] private ExecuteHolder _executeHolder;
    [SerializeField] private PhaseSettings _settings;

    private Phase _currentPhase;
    public Phase CurrentPhase
    {
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

            Dispatch(new EventObject
            {
                Sender = this,
                Type = PhaseChanged,
                Data = value
            });
        }
    }

    private int _playerStamina;
    public int PlayerStamina
    {
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
    public int CurrentStaminaCost
    {
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

    private readonly List<IPhaseItem> _obstacles = new List<IPhaseItem>();
    private readonly List<IPhaseItem> _enemies = new List<IPhaseItem>();

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

    public void RegisterObstacle(IPhaseItem obstacle)
    {
        _obstacles.Add(obstacle);
    }

    public void DeregisterObstacle(IPhaseItem obstacle)
    {
        _obstacles.Remove(obstacle);
    }

    public void RegisterEnemy(IPhaseItem enemy)
    {
        _enemies.Add(enemy);
    }

    public void DeregisterEnemy(IPhaseItem enemy)
    {
        _enemies.Remove(enemy);
    }

    public void ExecuteCards()
    {
        var cards = _executeHolder.Cards;
        CardManager.Instance.ExecuteCards(cards);

        CardManager.Instance.AddEventListener(CardManager.ExecutionDone, _ =>
        {
            CurrentPhase = Phase.Enemy;
            HandleCurrentPhase();
        }, true);
    }

    public void ShuffleCards()
    {
        CardManager.Instance.ShuffleCards();

        CurrentPhase = Phase.Enemy;
        HandleCurrentPhase();
    }

    private void HandleCurrentPhase()
    {
        switch (CurrentPhase)
        {
            case Phase.Player:
                // User controlled
                break;
            case Phase.Enemy:
                StartCoroutine(EnemyPhase());
                break;
            case Phase.Obstacles:
                StartCoroutine(ObstaclePhase());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator EnemyPhase()
    {
        for (var i = _enemies.Count - 1; i >= 0; i--)
        {
            var enemy = (Enemy)_enemies[i];
            var times = enemy.Moves;
            while (times > 0)
            {
                enemy.AdvanceStage();
                yield return new WaitUntil(() => !enemy.IsMoving || enemy.IsDieing);

                times--;
            }
        }

        yield return new WaitForSeconds(0.25f);

        CurrentPhase = Phase.Obstacles;
        HandleCurrentPhase();
    }

    private IEnumerator ObstaclePhase()
    {
        _obstacles.ForEach(obstacle => obstacle.AdvanceStage());

        yield return new WaitForSeconds(0.65f);

        CurrentPhase = Phase.Player;
        HandleCurrentPhase();
    }
}
