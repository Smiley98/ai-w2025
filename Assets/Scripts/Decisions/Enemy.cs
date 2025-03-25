using UnityEngine;

public class Enemy : MonoBehaviour
{
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
    }

    void Update()
    {
        Vector2 force = Steering.FollowLine(gameObject, waypoints, ref curr, ref next, ahead, speed);
        transform.up = rb.linearVelocity.normalized;
        rb.AddForce(force);
    }
}
