using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(Collider2D))]
public class Player : MonoBehaviour
{
    public PlayerMovement Movement { get; set; }

    private Collider2D _collider;

    private void Awake()
    {
        Movement = GetComponent<PlayerMovement>();
        _collider = GetComponent<Collider2D>();
    }

    public Enemy[] GetNearbyEnemies(bool diagonal = false)
    {
        _collider.enabled = false;

        Collider2D[] hits;
        if (diagonal)
            hits = Physics2D.OverlapBoxAll(transform.position + Vector3.one * 0.5f, Vector2.one * 2.2f, 0);
        else
            hits = Physics2D.OverlapCircleAll(transform.position + Vector3.one * 0.5f, 0.6f);               

        var enemies = hits.Select(hit => hit.transform.GetComponent<Enemy>()).Where(item => item != null).ToArray();

        _collider.enabled = true;

        return enemies;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        var collider = GetComponent<Collider2D>();
        Gizmos.DrawWireSphere(collider.bounds.center, 0.6f);
        Gizmos.DrawWireCube(collider.bounds.center, Vector2.one * 2.2f);
    }
}
