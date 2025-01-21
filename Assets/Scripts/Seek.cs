using UnityEngine;

// Apply this algorithm to the Steering.Seek method
public class Seek : MonoBehaviour
{
    public GameObject target;
    float moveSpeed = 10.0f;
    Rigidbody2D rb;

    Vector2 CurveSeek()
    {
        Vector2 currentVelocity = rb.linearVelocity;
        Vector2 desiredVelocity = (target.transform.position - transform.position).normalized * moveSpeed;
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

        //Vector2 seekForce = CurveSeek();
        //rb.AddForce(seekForce);

        Vector2 seekForce = Steering.Seek(rb, target.transform.position, moveSpeed, turnSpeed);
        rb.AddForce(seekForce);
    }
}
