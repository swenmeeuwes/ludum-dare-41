using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteCardVisitor : CardVisitor
{
    private readonly CardManager _cardManager;
    private readonly Player _player;

    public ExecuteCardVisitor(CardManager cardManager, Player player)
    {
        _cardManager = cardManager;
        _player = player;
    }

    public void Visit(MoveCard card)
    {
        var moveVector = Vector2Int.zero;
        switch (card.Direction)
        {
            case MoveDirection.Up:
                moveVector.y = card.Moves;
                break;
            case MoveDirection.Down:
                moveVector.y = -card.Moves;
                break;
            case MoveDirection.Right:
                moveVector.x = card.Moves;
                break;
            case MoveDirection.Left:
                moveVector.x = -card.Moves;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _player.Movement.Move(moveVector);

        card.Destroy();
        _cardManager.PutRandomCardInHand();
    }
}
