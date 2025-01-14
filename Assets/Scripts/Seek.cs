using UnityEngine;

public class Seek : MonoBehaviour
{
    public GameObject target;
    float speed = 25.0f;
    Rigidbody2D rb;

    // Outputs the *position change* to move FROM seeker TO target in a line
    Vector3 LineSeek()
    {
        // FROM seeker TO target
        Vector2 direction = (target.transform.position - transform.position).normalized;
        Vector2 change = direction * speed * Time.deltaTime;
        return change;
    }

    Vector2 CurveSeek()
    {
        Vector2 currentVelocity = rb.linearVelocity;
        Vector2 desiredVelocity = (target.transform.position - transform.position).normalized * speed;
        Vector2 seekForce = desiredVelocity - currentVelocity;
        return seekForce;
    }

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
