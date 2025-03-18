using UnityEngine;

public static class Steering
{
    public static Vector2 Seek(Rigidbody2D seeker, Vector2 target, float moveSpeed, float turnSpeed)
    {
        Vector2 desiredVelocity = (target - seeker.position).normalized * moveSpeed;
        return desiredVelocity - seeker.linearVelocity;
    }
}
