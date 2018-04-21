using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
        var gridManager = GridManager.Instance;
        while (moves.x != 0)
        {
            if (moves.x > 0)
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.right))
                    return;

                transform.position = GridPosition + Vector3Int.right;

                moves.x--;
            }
            else
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.left))
                    return;

                transform.position = GridPosition + Vector3Int.left;

                moves.x++;
            }
        }

        while (moves.y != 0)
        {
            if (moves.y > 0)
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.up))
                    return;

                transform.position = GridPosition + Vector3Int.up;

                moves.y--;
            }
            else
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.down))
                    return;

                transform.position = GridPosition + Vector3Int.down;

                moves.y++;
            }
        }
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