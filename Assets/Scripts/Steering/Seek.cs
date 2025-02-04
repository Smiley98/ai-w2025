using UnityEngine;

// Apply this algorithm to the Steering.Seek method
public class Seek : MonoBehaviour
{
    float moveSpeed = 10.0f;
    Rigidbody2D rb;

    // Optional: Add a strength parameter to replace moveSpeed so you can control seek vs avoidance force
    Vector2 CurveSeek(Vector3 target)
    {
        Vector2 currentVelocity = rb.linearVelocity;
        Vector2 desiredVelocity = (target - transform.position).normalized * moveSpeed;
        Vector2 seekForce = desiredVelocity - currentVelocity;
        return seekForce;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0.0f;
        Vector3 mouseDirection = (mouse - transform.position).normalized;
        float mouseAngle = Vector3.SignedAngle(Vector3.right, mouseDirection, Vector3.forward);
        
        // Rotate at 720 degrees per second
        float turnSpeed = 720.0f * Time.deltaTime;
        
        // Apply rotation from current (rb rotation) to desired (mouse angle) at a maximum rate of angular velocity (turn speed)
        rb.MoveRotation(Mathf.MoveTowardsAngle(rb.rotation, mouseAngle, turnSpeed));
        
        // Draw a line in the direction the seeker (rb) is facing, as a result of rb.MoveRotation from rb.rotation to the mouse cursor
        Vector3 direction = Quaternion.Euler(0.0f, 0.0f, rb.rotation) * Vector3.right;
        Debug.DrawLine(transform.position, transform.position + direction * 5.0f);

        // Instantaneously snaps the rotation to the input angle
        //rb.MoveRotation(angle);

        // Apply seek force, add avoidance force if obstacle detected
        Vector2 netForce = CurveSeek(mouse);
        float rayLength = 5.0f;
        Vector3 rayDirLeft = Quaternion.Euler(0.0f, 0.0f, 20.0f) * mouseDirection;
        Vector3 rayDirRight = Quaternion.Euler(0.0f, 0.0f, -20.0f) * mouseDirection;
        Vector2 rayEndLeft = transform.position + rayDirLeft * rayLength;
        Vector2 rayEndRight = transform.position + rayDirRight * rayLength;
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, rayEndLeft);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, rayEndRight);
        if (hitLeft)
        {
            // Avoid to the right
            Debug.Log("Left hit: " + hitLeft.collider.name);
            netForce += CurveSeek(-transform.up * rayLength);
        }
        else if (hitRight)
        {
            // Avoid to the left
            Debug.Log("Right hit: " + hitRight.collider.name);
            netForce += CurveSeek(transform.up * rayLength);
        }
        rb.AddForce(netForce);

        Debug.DrawLine(transform.position, rayEndLeft, Color.blue);
        Debug.DrawLine(transform.position, rayEndRight, Color.magenta);
        Debug.DrawLine(transform.position, transform.position + transform.right * rayLength, Color.red);
        Debug.DrawLine(transform.position, transform.position + transform.up * rayLength, Color.green);
    }
}
