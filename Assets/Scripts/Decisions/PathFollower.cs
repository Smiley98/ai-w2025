using UnityEngine;
using UnityEngine.InputSystem;

public class PathFollower : MonoBehaviour
{
    // Logic variables
    [SerializeField]
    GameObject[] waypoints;
    int curr = 0;
    int next = 1;

    // Physics variables
    Rigidbody2D rb;
    float speed = 10.0f;
    Vector2 direction = Vector2.zero;

    [SerializeField]
    GameObject projVisCurr;

    [SerializeField]
    GameObject projVisNext;

    float lookAhead = 2.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 A = waypoints[curr].transform.position;
        Vector2 B = waypoints[next].transform.position;
        Vector2 ahead = (Vector2)transform.position + (B - A).normalized * lookAhead;
        Vector2 proj = Projection.ProjectPointLine(A, B, ahead);
        rb.AddForce(Steering.Seek(rb, proj, 10.0f, 0.0f));

        // Check if our look-ahead exceeds line AB
        float t = Projection.ScalarProjectPointLine(A, B, ahead);

        // If so, seek next set of waypoints
        if (t > 1.0f)
        {
            ++curr;
            ++next;
            curr %= waypoints.Length;
            next %= waypoints.Length;
        }
    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    curr++;
    //    next++;
    //    curr = curr % waypoints.Length;
    //    next = next % waypoints.Length;
    //    transform.position = waypoints[curr].transform.position;
    //    direction = (waypoints[next].transform.position - waypoints[curr].transform.position).normalized;
    //    rb.linearVelocity = direction * speed;
    //}
}
