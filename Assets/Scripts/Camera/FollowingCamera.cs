using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo: Add locking of axis
[RequireComponent(typeof(Camera))]
public class FollowingCamera : MonoBehaviour
{
    public GameObject Target;
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
	}
	
	private void Update()
	{
	    var dampedPosition = Vector2.SmoothDamp(_followingCamera.transform.position, Target.transform.position,
	        ref _currentVelocity, SmoothTime, MaxSpeed, Time.deltaTime);

	    var newPosition = new Vector3(dampedPosition.x, dampedPosition.y, _startPos.z);
        _followingCamera.transform.position = newPosition;
	}
}
