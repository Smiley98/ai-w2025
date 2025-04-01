using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Sensors
    [SerializeField]
    GameObject player;

    // Path following
    [SerializeField]
    GameObject[] waypoints;
    int curr = 0;
    int next = 1;

    // Physics
    Rigidbody2D rb;
    float speed = 10.0f;
    float ahead = 2.0f;

    enum State
    {
        PATROL,
        ATTACK
    }

    // Behaviour
    State state = State.PATROL;
    float playerDetectRadius = 5.0f;
    float obstacleDetectRadius = 2.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //UpdateState();
        //switch (state)
        //{
        //    case State.PATROL:
        //        Patrol();
        //        break;
        //
        //    case State.ATTACK:
        //        Attack();
        //        break;
        //}

        //RaycastHit2D obstacleRaycast = Physics2D.Raycast(transform.position, toPlayer, playerDetectRadius);
        // Optional: add additional debug visualizations
        //Debug.DrawLine(transform.position, transform.position + transform.up * 5.0f);

        // Task: Find a nice way to rotate given from/to directions
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0.0f;
        float angularVelocity = 100.0f * Mathf.Deg2Rad;
        Vector3 from = transform.up;
        Vector3 to = (mouse - transform.position).normalized;
        transform.up = Vector3.RotateTowards(from, to, angularVelocity * Time.deltaTime, 0.0f).normalized;
        Debug.DrawLine(transform.position, transform.position + transform.up * 5.0f, Color.green);
    }

    void UpdateState()
    {
        // AB = B - A
        Vector3 toPlayer = (player.transform.position - transform.position).normalized;
        RaycastHit2D playerRaycast = Physics2D.Raycast(transform.position, toPlayer, playerDetectRadius);

        // In the future we may want to separate distance vs line-of-sight
        bool playerDetected = Vector2.Distance(player.transform.position, transform.position) <= playerDetectRadius;
        bool playerVisible = playerRaycast && playerRaycast.collider.CompareTag("Player");
        state = playerVisible ? State.ATTACK : State.PATROL;
    }

    void Patrol()
    {
        Vector2 force = Steering.FollowLine(gameObject, waypoints, ref curr, ref next, ahead, speed);
        transform.up = rb.linearVelocity.normalized;
        rb.AddForce(force);
    }

    void Attack()
    {
        Vector2 force = Steering.Seek(rb, player.transform.position, speed);
        transform.up = rb.linearVelocity.normalized;
        rb.AddForce(force);
    }
}
