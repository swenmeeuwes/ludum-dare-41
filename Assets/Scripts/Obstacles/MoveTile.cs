using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveTileDirection
{
    Up,
    Left,
    Down,
    Right
}

[RequireComponent(typeof(Collider2D))]
public class MoveTile : MonoBehaviour, IPhaseItem
{
    [SerializeField] private MoveTileDirection _initialDirection;

    private MoveTileDirection _direction;

    public MoveTileDirection Direction
    {
        get { return _direction; }
        set
        {
            var possibleDirections = Enum.GetNames(typeof(MoveTileDirection));
            // Get index
            int newDirectionIndex = 0;
            for (var i = 0; i < possibleDirections.Length; i++)
            {
                if (possibleDirections[i] != value.ToString())
                    continue;

                newDirectionIndex = i;
                break;
            }

            transform.rotation = Quaternion.Euler(0, 0, 90f * newDirectionIndex);
            _direction = value;
        }
    }

    private Collider2D _collider;

    private Transform _holding; // Transform currently standing on move tile

    private void Start()
    {
        _collider = GetComponent<Collider2D>();

        Direction = _initialDirection;

        PhaseManager.Instance.RegisterObstacle(this);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>() == null && collider.GetComponent<Enemy>() == null)
            return;

        _holding = collider.transform;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>() == null && collider.GetComponent<Enemy>() == null)
            return;

        _holding = null;
    }

    public void AdvanceStage()
    {
        if (_holding == null)
            return;

        Vector3Int move;
        switch (Direction)
        {
            case MoveTileDirection.Up:
                move = Vector3Int.up;
                break;
            case MoveTileDirection.Left:
                move = Vector3Int.left;
                break;
            case MoveTileDirection.Down:
                move = Vector3Int.down;
                break;
            case MoveTileDirection.Right:
                move = Vector3Int.right;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _holding.position += move;
    }
}
