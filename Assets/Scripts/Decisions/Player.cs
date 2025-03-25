using UnityEngine;

public class Player : MonoBehaviour
{
    void Update()
    {
        Vector3 direction = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        direction = direction.normalized;

        const float speed = 25.0f;
        transform.position += direction * speed * Time.deltaTime;
    }
}
