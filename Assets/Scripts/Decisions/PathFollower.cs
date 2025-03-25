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
    float moveSpeed = 10.0f;    // linear velocity = 10 units per second
    float turnSpeed = 100.0f;   // angular velocity = 100 degrees per second
    float ahead = 2.0f;         // How far to look ahead along the projected path

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

        Vector2 force = Steering.FollowLine(gameObject, waypoints, ref curr, ref next, ahead, moveSpeed, turnSpeed);
        rb.AddForce(force);
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
        rb.linearVelocity = (waypoints[next].transform.position - waypoints[curr].transform.position).normalized * moveSpeed;
    }
}
