using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoEventDispatcher
{
    public static readonly string IsMovingChanged = "PlayerMovement.IsMovingChanged";

    private bool _isBusy;
    public bool IsBusy {
        get { return _isBusy; }
        set
        {
            _isBusy = value;
            Dispatch(new EventObject
            {
                Sender = this,
                Type = IsMovingChanged,
                Data = value
            });
        }        
    }

    public Vector3Int GridPosition
    {
        get { return Vector3Int.FloorToInt(transform.position); }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += Vector3.left * GridManager.Instance.GridSize;
        }
    }

    public void Move(Vector2Int moves)
    {
        StartCoroutine(MoveCoroutine(moves));
    }

    public IEnumerator MoveCoroutine(Vector2Int moves)
    {
        IsBusy = true;

        var gridManager = GridManager.Instance;
        while (moves.x != 0)
        {
            if (moves.x > 0)
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.right))
                    break;

                transform.position = GridPosition + Vector3Int.right;

                moves.x--;
            }
            else
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.left))
                    break;

                transform.position = GridPosition + Vector3Int.left;

                moves.x++;
            }

            yield return new WaitForSeconds(0.35f);
        }

        while (moves.y != 0)
        {
            if (moves.y > 0)
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.up))
                    break;

                transform.position = GridPosition + Vector3Int.up;

                moves.y--;
            }
            else
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.down))
                    break;

                transform.position = GridPosition + Vector3Int.down;

                moves.y++;
            }

            yield return new WaitForSeconds(0.35f);
        }

        IsBusy = false;
    }

    [Obsolete]
    private HashSet<Vector3Int> GetPossibleMoves(int range)
    {
        var possibleMoves = new HashSet<Vector3Int>();
        GetPossibleMovesRec(ref possibleMoves, GridPosition, range + 1); // + 1 for current GridPosition, which is ignored

        return possibleMoves;
    }

    [Obsolete]
    private void GetPossibleMovesRec(ref HashSet<Vector3Int> possibleMoves, Vector3Int position, int power)
    {
        if (power == 0)
            return;

        if (position != GridPosition)
        {            
            var tile = GridManager.Instance.GetTilesOn(position);
            if (tile.Length == 0)
                possibleMoves.Add(position);
        }

        // Check adjacent tiles
        GetPossibleMovesRec(ref possibleMoves, position - Vector3Int.up, power - 1);
        GetPossibleMovesRec(ref possibleMoves, position - Vector3Int.down, power - 1);
        GetPossibleMovesRec(ref possibleMoves, position - Vector3Int.left, power - 1);
        GetPossibleMovesRec(ref possibleMoves, position - Vector3Int.right, power - 1);
    }
}