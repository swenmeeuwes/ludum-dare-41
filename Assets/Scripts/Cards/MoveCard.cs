using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum MoveDirection
{
    Up,
    Down,
    Right,
    Left
}

public class DirectionProbabilityItem
{
    public MoveDirection Direction { get; set; }
    public int Probability { get; set; }
}

public class MoveCard : Card
{
    //[SerializeField] private Sprite[] _sprites; // In order of enum
    [SerializeField] private Image _directionImage;

    private int _move;
    public int Moves
    {
        get { return _move; }
        set
        {
            Elements.Description.text = string.Format("Move {0} spaces {1}", value, Direction);
            _move = value;
            Cost = value;
        }
    }

    private MoveDirection _direction;
    public MoveDirection Direction
    {
        get { return _direction; }
        set
        {
            _directionImage.rectTransform.rotation = Quaternion.Euler(0, 0, DirectionToRotation(value));
            Elements.Header.text = string.Format("Move {0}", value);
            _direction = value;
        }
    }

    public override void Accept(CardVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override void Randomize()
    {
        // Randomize direction
        //var moveDirections = Enum.GetNames(typeof(MoveDirection));
        //var randomDirection = moveDirections[UnityEngine.Random.Range(0, moveDirections.Length)];
        //Direction = (MoveDirection)Enum.Parse(typeof(MoveDirection), randomDirection);
        Direction = ComputeDirection();

        // Randomize moves
        Moves = UnityEngine.Random.Range(1, 3); // 1 - 2 moves (3 = exclusive)

        // 5% chance on 3 move cards
        if (UnityEngine.Random.value > 0.95f)
            Moves = 3;
    }

    private MoveDirection ComputeDirection()
    {
        var moveDirections = Enum.GetNames(typeof(MoveDirection));
        var probabilitySet = new DirectionProbabilityItem[moveDirections.Length];
        var playerDifferenceToGoal = GameManager.Instance.ComputeDifferenceBetweenPlayerAndGoal();

        // HACKY for now
        //Up,
        //Down,
        //Right,
        //Left
        probabilitySet[0] = new DirectionProbabilityItem
        {
            Direction = MoveDirection.Up,
            Probability = playerDifferenceToGoal.y > 0 ? 30 : 20
        };

        probabilitySet[1] = new DirectionProbabilityItem
        {
            Direction = MoveDirection.Down,
            Probability = playerDifferenceToGoal.y < 0 ? 30 : 20
        };

        probabilitySet[2] = new DirectionProbabilityItem
        {
            Direction = MoveDirection.Right,
            Probability = playerDifferenceToGoal.x > 0 ? 30 : 20
        };

        probabilitySet[3] = new DirectionProbabilityItem
        {
            Direction = MoveDirection.Left,
            Probability = playerDifferenceToGoal.x < 0 ? 30 : 20
        };

        var probabilityCountDown = Random.Range(0, 100);
        for (var i = 0; i < probabilitySet.Length; i++)
        {
            probabilityCountDown -= probabilitySet[i].Probability;
            if (probabilityCountDown < 0)
                return probabilitySet[i].Direction;
        }

        return probabilitySet[probabilitySet.Length - 1].Direction;
    }

    private int DirectionToRotation(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Up:
                return 0;
            case MoveDirection.Left:
                return 90;
            case MoveDirection.Down:
                return 180;
            case MoveDirection.Right:
                return 270;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }
}
