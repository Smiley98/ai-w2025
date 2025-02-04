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

    // Lab Exercise 3:
    // Task 1: Make a function that calculates avoidance force similar to the above Seek function
    // It should be a public static method so avoidance can be applied to any object with a Rigidbody2D - 1%
    // Task 2: Add and test 2 more avoidance rays, resulting in a total of 4 rays - 2%
}
