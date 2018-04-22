using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo: Add locking of axis
[RequireComponent(typeof(Camera))]
public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private bool _smooth; // Testing for pixel perfectness

    public Transform Target;
    public float SmoothTime;
    public float MaxSpeed;

    private Camera _followingCamera;
    private Vector3 _startPos;

    #region References
    private Vector2 _currentVelocity;
    #endregion

    private void Awake()
    {
        _startPos = transform.position;
    }

    private void Start()
    {
        _followingCamera = GetComponent<Camera>();

        if (Target == null)
            Target = FindObjectOfType<Player>().transform;

        // Start at target
        var newPosition = Target.transform.position;
        newPosition.z = transform.position.z;

        _followingCamera.transform.position = Vector3Int.RoundToInt(newPosition);
    }

    private void Update()
    {
        if (_smooth)
        {
            var dampedPosition = Vector2.SmoothDamp(_followingCamera.transform.position, Vector2Int.RoundToInt(Target.transform.position),
                ref _currentVelocity, SmoothTime, MaxSpeed, Time.deltaTime);

            var newPosition = new Vector3(dampedPosition.x, dampedPosition.y, _startPos.z);
            _followingCamera.transform.position = newPosition;
        }
        else
        {
            var newPosition = Target.transform.position;
            newPosition.z = transform.position.z;

            _followingCamera.transform.position = Vector3Int.RoundToInt(newPosition);
        }
    }
}
