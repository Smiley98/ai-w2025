using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum State
    {
        PATROL,
        ATTACK
    }

    State state = State.PATROL;

    [SerializeField]
    GameObject player;

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
        // Better gameplay idea:
        // Instead of shooting a ray where the enemy is looking, shoot a ray towards the player.
        // Next week we will do this. We will add obstacles to the scene so the player isn't always visible!
        // We will also add combat!!!

        bool hit = Physics2D.Raycast(transform.position, transform.up);
        //float distance = Vector2.Distance(player.transform.position, transform.position);
        //state = distance <= 3.0f ? State.ATTACK : State.PATROL;
        state = hit ? State.ATTACK : State.PATROL;
        Debug.DrawLine(transform.position, transform.position + transform.up * 10.0f, hit ? Color.red : Color.green);

        switch (state)
        {
            case State.PATROL:
                Patrol();
                break;

            case State.ATTACK:
                Attack();
                break;
        }
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
