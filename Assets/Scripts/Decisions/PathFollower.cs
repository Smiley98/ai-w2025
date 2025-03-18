using UnityEngine;

public class PathFollower : MonoBehaviour
{
    bool linear = true;

    // Logic variables
    [SerializeField]
    GameObject[] waypoints;
    int curr = -1;
    int next = 0;

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
        if (linear)
            OnLinearEnter();
    }

    void Update()
    {
        // Toggle linear vs curved path following
        if (Input.GetKeyDown(KeyCode.F))
        {
            linear = !linear;
            if (linear)
                OnLinearEnter();
        }

        // Only run if curved
        if (linear)
            return;

        // Calculate seek target
        Vector2 A = waypoints[curr].transform.position;
        Vector2 B = waypoints[next].transform.position;
        Vector2 projCurr = Projection.ProjectPointLine(A, B, transform.position);
        Vector2 projNext = projCurr + (B - A).normalized * lookAhead;
        rb.AddForce(Steering.Seek(rb, projNext, 10.0f, 0.0f));

        // Visualize projections
        projVisCurr.transform.position = projCurr;
        projVisNext.transform.position = projNext;

        // Update waypoints if our look-ahead projection exceeds line AB
        float t = Projection.ScalarProjectPointLine(A, B, projNext);
        if (t > 1.0f)
        {
            ++curr;
            ++next;
            curr %= waypoints.Length;
            next %= waypoints.Length;
        }
    }

    void OnLinearEnter()
    {
        curr = -1;
        next = 0;
        transform.position = waypoints[0].transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Only run if linear
        if (!linear)
            return;

        curr++;
        next++;
        curr = curr % waypoints.Length;
        next = next % waypoints.Length;
        transform.position = waypoints[curr].transform.position;
        direction = (waypoints[next].transform.position - waypoints[curr].transform.position).normalized;
        rb.linearVelocity = direction * speed;

        projVisCurr.transform.position = waypoints[curr].transform.position;
        projVisNext.transform.position = waypoints[next].transform.position;
    }
}
