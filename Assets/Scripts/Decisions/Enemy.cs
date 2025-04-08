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
    float speed = 5.0f;
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
        UpdateState();
        switch (state)
        {
            case State.PATROL:
                Patrol();
                break;
        
            case State.ATTACK:
                Attack();
                break;
        }
        Avoid();
        UpdateAngularVelocity();
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

    // Seek waypoints
    void Patrol()
    {
        Vector2 force = Steering.FollowLine(gameObject, waypoints, ref curr, ref next, ahead, speed);
        rb.AddForce(force);
    }

    // Seek player
    void Attack()
    {
        Vector2 force = Steering.Seek(rb, player.transform.position, speed);
        rb.AddForce(force);
    }

    // Steer away from obstacles
    void Avoid()
    {
        Vector3 dirLeft = Quaternion.Euler(0.0f, 0.0f, 20.0f) * transform.up;
        Vector3 dirRight = Quaternion.Euler(0.0f, 0.0f, -20.0f) * transform.up;
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, dirLeft, obstacleDetectRadius);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, dirRight, obstacleDetectRadius);
        if (hitLeft && hitLeft.collider.CompareTag("Obstacle"))
        {
            // Turn right
            rb.AddForce(Steering.Seek(rb, transform.right * obstacleDetectRadius, speed));
        }
        else if (hitRight && hitRight.collider.CompareTag("Obstacle"))
        {
            // Turn left
            rb.AddForce(Steering.Seek(rb, -transform.right * obstacleDetectRadius, speed));
        }
    }

    void UpdateAngularVelocity()
    {
        float av = 250.0f * Mathf.Deg2Rad * Time.deltaTime;
        transform.up = Vector3.RotateTowards(transform.up, rb.linearVelocity.normalized, av, 0.0f).normalized;
        Debug.DrawLine(transform.position, transform.position + transform.up * 5.0f);
    }
}

// Smooth-rotation towards mouse test:
//Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//mouse.z = 0.0f;
//float angularVelocity = 100.0f * Mathf.Deg2Rad;
//Vector3 from = transform.up;
//Vector3 to = (mouse - transform.position).normalized;
//transform.up = Vector3.RotateTowards(from, to, angularVelocity * Time.deltaTime, 0.0f).normalized;
//Debug.DrawLine(transform.position, transform.position + transform.up * 5.0f, Color.green);
