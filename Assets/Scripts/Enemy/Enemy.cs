using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPhaseItem
{
    [SerializeField] private ParticleSystem _bloodParticleSystem;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public Vector3Int GridPosition
    {
        get { return Vector3Int.FloorToInt(transform.position); }
    }

    public bool IsDieing { get; set; }
    public bool IsMoving { get; set; }
    public int Moves = 1;

    private Player _target;

    private void Start()
    {
        PhaseManager.Instance.RegisterEnemy(this);

        _target = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>() != null)
        {
            GameManager.Instance.State = GameState.GameOver;
        }

        if (collider.GetComponent<Spikes>() != null)
        {
            StartCoroutine(DieCoroutine());
        }
    }

    public void Attacked(PlayerMovement player)
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        IsDieing = true;

        _spriteRenderer.enabled = false;
        _bloodParticleSystem.Play();

        yield return new WaitForSeconds(5f); //hack

        PhaseManager.Instance.DeregisterEnemy(this);

        Destroy(gameObject);
    }

    public void AdvanceStage()
    {
        if (IsDieing)
            return;

        var directionToPlayer = Vector3Int.CeilToInt(_target.transform.position - transform.position);
        directionToPlayer.Clamp(new Vector3Int(-1, -1, -1), Vector3Int.one);

        Move(new Vector2Int(directionToPlayer.x, directionToPlayer.y));
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
                if (!gridManager.IsFree(GridPosition + Vector3Int.right, GridLayer.Walls) || !IsEntityFree(GridPosition + Vector3Int.right))
                    break;

                transform.localPosition = GridPosition + Vector3Int.right;
                Rotate(Vector2Int.right);

                moves.x--;
            }
            else
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.left, GridLayer.Walls) || !IsEntityFree(GridPosition + Vector3Int.left))
                    break;

                transform.localPosition = GridPosition + Vector3Int.left;
                Rotate(Vector2Int.left);

                moves.x++;
            }

            SoundManager.Instance.Play(Sound.Walk);

            yield return new WaitForSeconds(0.35f);

            IsMoving = false;
            yield break;
        }

        while (moves.y != 0)
        {
            if (moves.y > 0)
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.up, GridLayer.Walls) || !IsEntityFree(GridPosition + Vector3Int.up))
                    break;

                transform.localPosition = GridPosition + Vector3Int.up;
                Rotate(Vector2Int.up);

                moves.y--;
            }
            else
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.down, GridLayer.Walls) || !IsEntityFree(GridPosition + Vector3Int.down))
                    break;

                transform.localPosition = GridPosition + Vector3Int.down;
                Rotate(Vector2Int.down);

                moves.y++;
            }

            SoundManager.Instance.Play(Sound.Walk);

            yield return new WaitForSeconds(0.35f);

            IsMoving = false;
            yield break;
        }

        IsMoving = false;
    }

    private bool IsEntityFree(Vector3Int position)
    {           
        var hit = Physics2D.OverlapBox(new Vector2(position.x - 0.5f, position.y - 0.5f), Vector2.one * 1f, 0);
        if (hit == null)
            return true;

        var spikes = hit.GetComponent<Spikes>();
        if (spikes != null && spikes.CurrentStage != spikes.Stages - 1)
            return true;

        var moveTile = hit.GetComponent<MoveTile>();
        if (moveTile != null)
            return true;

        return false;
    }

    private void Rotate(Vector2Int direction)
    {
        var rotation = DirectionUtil.DirectionToRotation(direction);
        _spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
