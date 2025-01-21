using UnityEngine;

public class Seek : MonoBehaviour
{
    public GameObject target;
    float speed = 10.0f;
    Rigidbody2D rb;

    Vector2 CurveSeek()
    {
        Vector2 currentVelocity = rb.linearVelocity;
        Vector2 desiredVelocity = (target.transform.position - transform.position).normalized * speed;
        Vector2 seekForce = desiredVelocity - currentVelocity;
        return seekForce;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.angularVelocity = 100.0f;
        Vector3 direction = Quaternion.Euler(0.0f, 0.0f, rb.rotation) * Vector3.right;
        Debug.DrawLine(transform.position, transform.position + direction * 5.0f);

        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0.0f;
        Vector3 mouseDirection = (mouse - transform.position).normalized;

        // Rotate at 720 degrees per second
        float angularVelocity = 720.0f * Time.deltaTime;

        // Angle from "zero" (aka Vector3.right which is the horizontal) to our mouse (+z is the axis of rotation since we're in 2d, hence Vector3.forward)
        float angle = Vector3.SignedAngle(Vector3.right, mouseDirection, Vector3.forward);

        // Apply rotation from current to desired at a maximum rate of angular velocity
        rb.MoveRotation(Mathf.MoveTowardsAngle(rb.rotation, angle, angularVelocity));
        
        // Instantaneously snaps the rotation to the input angle
        //rb.MoveRotation(angle);

        Vector2 seekForce = CurveSeek();
        rb.AddForce(seekForce);
    }
}
