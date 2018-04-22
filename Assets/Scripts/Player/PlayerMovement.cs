using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoEventDispatcher
{
    public static readonly string IsMovingChanged = "PlayerMovement.IsMovingChanged";

    [SerializeField] private SpriteRenderer _spriteRenderer;

    private bool _isMoving;
    public bool IsMoving {
        get { return _isMoving; }
        set
        {
            _isMoving = value;
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

    public void Attack(Enemy[] enemies)
    {
        StartCoroutine(AttackCoroutine(enemies));
    }

    public IEnumerator AttackCoroutine(Enemy[] enemies)
    {
        IsMoving = true;

        foreach (var enemy in enemies)
        {
            enemy.Attacked(this);
            SoundManager.Instance.Play(Sound.Attack);

            yield return new WaitForSeconds(0.1f);
        }        

        yield return new WaitForSeconds(0.25f);

        IsMoving = false;
    }

    public void Move(Vector2Int moves)
    {
        StartCoroutine(MoveCoroutine(moves));
    }

    public IEnumerator MoveCoroutine(Vector2Int moves)
    {
        IsMoving = true;

        var gridManager = GridManager.Instance;
        while (moves.x != 0)
        {
            if (moves.x > 0)
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.right, GridLayer.Walls))
                    break;

                transform.localPosition = GridPosition + Vector3Int.right;
                Rotate(Vector2Int.right);

                moves.x--;
            }
            else
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.left, GridLayer.Walls))
                    break;

                transform.localPosition = GridPosition + Vector3Int.left;
                Rotate(Vector2Int.left);

                moves.x++;
            }

            SoundManager.Instance.Play(Sound.Walk);

            yield return new WaitForSeconds(0.35f);
        }

        while (moves.y != 0)
        {
            if (moves.y > 0)
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.up, GridLayer.Walls))
                    break;

                transform.localPosition = GridPosition + Vector3Int.up;
                Rotate(Vector2Int.up);

                moves.y--;
            }
            else
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.down, GridLayer.Walls))
                    break;

                transform.localPosition = GridPosition + Vector3Int.down;
                Rotate(Vector2Int.down);

                moves.y++;
            }

            SoundManager.Instance.Play(Sound.Walk);

            yield return new WaitForSeconds(0.35f);
        }

        IsMoving = false;
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

    private void Rotate(Vector2Int direction)
    {
        var rotation = DirectionUtil.DirectionToRotation(direction);
        _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}