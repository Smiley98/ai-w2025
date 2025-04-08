using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;

    void Update()
    {
        Move();
        Shoot();
    }

    void Move()
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

        const float speed = 15.0f;
        transform.position += direction * speed * Time.deltaTime;
    }

    void Shoot()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0.0f;

        Vector3 direction = (mouse - transform.position).normalized;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.localScale *= 0.25f;
            float bulletRadius = bullet.transform.localScale.x * 0.5f;
            float playerRadius = transform.localScale.x * 0.5f;

            bullet.transform.position = transform.position + direction * (playerRadius + bulletRadius);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 20.0f;
            Destroy(bullet, 1.0f);
        }
    }
}
