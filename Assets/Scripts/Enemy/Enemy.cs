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

    public bool IsMoving { get; set; }

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
        _spriteRenderer.enabled = false;
        _bloodParticleSystem.Play();

        PhaseManager.Instance.DeregisterEnemy(this);

        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject);
    }

    public void AdvanceStage()
    {
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
                if (!gridManager.IsFree(GridPosition + Vector3Int.right, GridLayer.Walls))
                    break;

                transform.localPosition = GridPosition + Vector3Int.right;

                moves.x--;
            }
            else
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.left, GridLayer.Walls))
                    break;

                transform.localPosition = GridPosition + Vector3Int.left;

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

                moves.y--;
            }
            else
            {
                if (!gridManager.IsFree(GridPosition + Vector3Int.down, GridLayer.Walls))
                    break;

                transform.localPosition = GridPosition + Vector3Int.down;

                moves.y++;
            }

            SoundManager.Instance.Play(Sound.Walk);

            yield return new WaitForSeconds(0.35f);
        }

        IsMoving = false;
    }    
}
