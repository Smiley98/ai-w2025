using UnityEngine;

public class Seek : MonoBehaviour
{
    public GameObject target;
    float speed = 10.0f;

    void Start()
    {
        
    }

    void Update()
    {
        // FROM seeker TO target
        Vector2 direction = (target.transform.position - transform.position).normalized;
        float dt = Time.deltaTime;
        Vector3 change = direction * speed * dt;
        transform.position += change;
    }
}
