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

    public Enemy[] GetNearbyEnemies()
    {
        _collider.enabled = false;

        var hits = Physics2D.CircleCastAll(transform.position, 0.1f, Vector2.one); // Changed to box for 8 sides
        var enemies = hits.Select(hit => hit.transform.GetComponent<Enemy>()).Where(item => item != null).ToArray();

        _collider.enabled = true;

        return enemies;
    }
}
