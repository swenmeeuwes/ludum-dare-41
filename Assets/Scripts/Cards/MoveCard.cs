using System;
using UnityEngine;

public enum MoveDirection
{
    Up,
    Down,
    Right,
    Left
}

public class MoveCard : Card
{
    private int _move;
    public int Moves {
        get { return _move; }
        set
        {
            Elements.Description.text = string.Format("Move {0} spaces {1}", value, Direction);
            _move = value;
            Cost = value;
        }
    }

    private MoveDirection _direction;
    public MoveDirection Direction {
        get { return _direction; }
        set
        {
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
        var moveDirections = Enum.GetNames(typeof(MoveDirection));
        var randomDirection = moveDirections[UnityEngine.Random.Range(0, moveDirections.Length)];
        Direction = (MoveDirection)Enum.Parse(typeof(MoveDirection), randomDirection);

        // Randomize moves
        Moves = UnityEngine.Random.Range(1, 3); // 1 - 2 moves (3 = exclusive)
    }
}
