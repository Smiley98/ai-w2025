using UnityEngine;

public class Seek : MonoBehaviour
{
    public GameObject target;
    float speed = 2.0f;
    Rigidbody2D rb;

    // Optional task 1: Make a separate GameObject that uses LineSeek to illustrate the difference between line vs curve seek
    // Outputs the *position change* to move FROM seeker TO target in a line
    Vector3 LineSeek()
    {
        // FROM seeker TO target
        Vector2 direction = (target.transform.position - transform.position).normalized;
        Vector2 change = direction * speed * Time.deltaTime;
        return change;
    }

    // Optional task 2: Add a function called CurveFlee that flees the target instead of seeking the target
    Vector2 CurveSeek()
    {
        Vector2 currentVelocity = rb.linearVelocity;
        Vector2 desiredVelocity = (target.transform.position - transform.position).normalized * speed;
        Vector2 seekForce = desiredVelocity - currentVelocity;
        return seekForce;
    }

    // Optional task 3: Write code to rotate the seeker in the direction of its velocity (2% in lab 2, due on the 26th)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Line seek is a velocity, so apply it directly to our seeker's position!
        //transform.position += LineSeek();

        Vector2 seekForce = CurveSeek();
        rb.AddForce(seekForce);
    }
}
