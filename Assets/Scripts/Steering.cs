using UnityEngine;

public static class Steering
{
    // "Seek is a static method that can be applied to any GameObject" -- Done for you here
    public static Vector2 Seek(Rigidbody2D seeker, Vector2 target, float moveSpeed, float turnSpeed)
    {
        // 2% -- Output seeking force (smooth acceleration leading to curved motion)
        // 2% -- Use seeker.MoveRotation to rotate the seeker towards its velocity
        // (Instead of rotating towards the mouse cursor, you need to rotate it towards its velocity direction)
        return Vector2.zero;
    }
}
