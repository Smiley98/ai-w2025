using UnityEngine;

public static class Steering
{
    public static Vector2 Seek(Rigidbody2D seeker, Vector2 target, float moveSpeed, float turnSpeed/*degrees per second*/)
    {
        // Our seeker should face towards where it should be going (desiredVelocity, seeker to target)
        //Vector2 currentVelocity = seeker.linearVelocity;
        //Vector2 desiredVelocity = (target - seeker.position).normalized * moveSpeed;
        //Vector2 acc = desiredVelocity - currentVelocity;

        //Vector2 desiredDirection = desiredVelocity.normalized;
        //Vector2 currentDirection = currentVelocity.normalized;
        //
        //Debug.DrawLine(seeker.position, seeker.position + currentDirection * 5.0f, Color.red);
        //Debug.DrawLine(seeker.position, seeker.position + desiredDirection * 5.0f, Color.green);

        // Uncomment for gradual instead of instantaneous turning
        //float angle = Vector2.SignedAngle(currentDirection, desiredDirection);
        //seeker.rotation = Mathf.MoveTowardsAngle(seeker.rotation, seeker.rotation + angle, turnSpeed);
        //Debug.Log(angle);

        // Simplest way to rotate an object in the direction of motion is to instantaneously point it in the direction of motion
        //seeker.transform.up = currentDirection;

        return (target - seeker.position).normalized * moveSpeed - seeker.linearVelocity;
    }

    public static Vector2 FollowLine(GameObject seeker, GameObject[] waypoints, ref int curr, ref int next, float ahead, float moveSpeed, float turnSpeed)
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

        return Seek(seeker.GetComponent<Rigidbody2D>(), projNext, moveSpeed, turnSpeed);
    }
}
