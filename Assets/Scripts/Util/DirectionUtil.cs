using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionUtil  {
    public static float DirectionToRotation(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
            return 0;
        if (direction == Vector2Int.left)
            return 90;
        if (direction == Vector2Int.down)
            return 180;
        if (direction == Vector2Int.right)
            return 270;

        return 0;
    }
}
