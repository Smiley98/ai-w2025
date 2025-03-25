using UnityEngine;

public class PathFollower : MonoBehaviour
{
    bool linear = false;

    // Logic variables
    [SerializeField]
    GameObject[] waypoints;
    int curr = 0;
    int next = 1;

    // Physics variables
    Rigidbody2D rb;
    float speed = 10.0f;
    float ahead = 2.0f;

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

        Vector2 force = Steering.FollowLine(gameObject, waypoints, ref curr, ref next, ahead, speed);
        transform.up = rb.linearVelocity.normalized;
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
        rb.linearVelocity = (waypoints[next].transform.position - waypoints[curr].transform.position).normalized * speed;
    }
}
