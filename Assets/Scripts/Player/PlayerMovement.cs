using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;

    private void Update()
    {
        var input = new Vector2(Input.GetAxis(InputAxesLiterals.Horizontal), Input.GetAxis(InputAxesLiterals.Vertical));
        transform.Translate(input * Speed * Time.deltaTime);
    }
}