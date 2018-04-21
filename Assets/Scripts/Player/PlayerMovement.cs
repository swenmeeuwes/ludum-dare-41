using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;

    private bool _isMoving;

    private void Update()
    {
        _isMoving = InputHandler.Instance.GetAction(InputAxesLiterals.Move);
        if (_isMoving)
            MoveHorizontallyToInputPosition();
    }

    private void MoveHorizontallyToInputPosition()
    {
        var screenPosition = InputHandler.Instance.GenericInputPosition;
        var worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        var targetPosition = worldPosition;
        targetPosition.y = transform.position.y; // Stay on same vertical position

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
    }
}