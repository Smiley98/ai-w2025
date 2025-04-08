using UnityEngine;

public enum Team
{
    NONE,
    PLAYER,
    ENEMY
}

public class Projectile : MonoBehaviour
{
    public Team team = Team.NONE;
    public float damage = 0.0f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
            return;

        if (team == Team.NONE)
            Debug.LogError("Uninitialized bullet team");

        if (damage <= 0.0f)
            Debug.LogError("Uninitialized bullet damage");

        // Damage the enemy
        if (team == Team.PLAYER && collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.health -= damage;
        }
        // Damage the player
        else if (team == Team.ENEMY && collision.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.health -= damage;
        }

        Destroy(gameObject);
    }
}
