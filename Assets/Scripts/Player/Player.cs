using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    public PlayerMovement Movement { get; set; }

    private void Awake()
    {
        Movement = GetComponent<PlayerMovement>();
    }
}
