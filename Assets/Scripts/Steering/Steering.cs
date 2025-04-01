using UnityEngine;

public static class Steering
{
    public static Vector2 Seek(Rigidbody2D seeker, Vector2 target, float moveSpeed/*, float turnSpeed*/)
    {
        return (target - seeker.position).normalized * moveSpeed - seeker.linearVelocity;
    }

    public static Vector2 FollowLine(GameObject seeker, GameObject[] waypoints, ref int curr, ref int next, float ahead, float speed)
    {
        // Calculate seek target
        Vector2 A = waypoints[curr].transform.position;
        Vector2 B = waypoints[next].transform.position;
        Vector2 projCurr = Projection.ProjectPointLine(A, B, seeker.transform.position);
        Vector2 projNext = projCurr + (B - A).normalized * ahead;

        // Update waypoints if our look-ahead projection exceeds line AB
        float t = Projection.ScalarProjectPointLine(A, B, projNext);
        if (t > 1.0f)
        {
            ++curr;
            ++next;
            curr %= waypoints.Length;
            next %= waypoints.Length;
        }

        return Seek(seeker.GetComponent<Rigidbody2D>(), projNext, speed);
    }
}
