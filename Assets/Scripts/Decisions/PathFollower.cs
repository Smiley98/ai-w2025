using UnityEngine;

public class PathFollower : MonoBehaviour
{
    // Logic variables
    [SerializeField]
    GameObject[] waypoints;
    int curr = -1;
    int next = 0;

    // Physics variables
    Rigidbody2D rb;
    float speed = 10.0f;
    Vector2 direction = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = waypoints[0].transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        curr++;
        next++;
        curr = curr % waypoints.Length;
        next = next % waypoints.Length;
        transform.position = waypoints[curr].transform.position;
        direction = (waypoints[next].transform.position - waypoints[curr].transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }
}
