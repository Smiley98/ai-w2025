using UnityEngine;

public abstract class Weapon
{
    public GameObject weaponPrefab;
    public GameObject owner;
    public Team team = Team.NONE;
    public float damage = 0.0f;

    public float timeCurrent = 0.0f;
    public float timeMax = 0.0f; // <-- how long we wait in-between shots (ie 0.1 for machine gun, 1.0 for sniper)

    public abstract void Shoot(Vector3 direction);

    protected GameObject CreateProjectile(float scale, Vector3 direction, float speed, Color color)
    {
        GameObject projectile = GameObject.Instantiate(weaponPrefab);
        projectile.transform.localScale *= scale;
        float projectileRadius = projectile.transform.localScale.x * 0.5f;
        float ownerRadius = owner.transform.localScale.x * 0.5f;

        projectile.transform.position = owner.transform.position + direction * (ownerRadius + projectileRadius);
        projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
        projectile.GetComponent<SpriteRenderer>().color = color;

        Projectile p = projectile.GetComponent<Projectile>();
        p.team = team;
        p.damage = damage;
        return projectile;
    }
}

public class Rifle : Weapon
{
    public override void Shoot(Vector3 direction)
    {
        timeCurrent += Time.deltaTime;
        if (timeCurrent >= timeMax)
        {
            timeCurrent = 0.0f;

            GameObject projectile = CreateProjectile(0.25f, direction, 20.0f, Color.red);
            GameObject.Destroy(projectile, 1.0f);
        }
    }
}

public class Shotgun : Weapon
{
    public override void Shoot(Vector3 direction)
    {
        timeCurrent += Time.deltaTime;
        if (timeCurrent >= timeMax)
        {
            timeCurrent = 0.0f;

            Vector3 dirLeft = Quaternion.Euler(0.0f, 0.0f, 20.0f) * direction;
            Vector3 dirRight = Quaternion.Euler(0.0f, 0.0f, -20.0f) * direction;

            GameObject projectile = CreateProjectile(0.25f, direction, 10.0f, Color.green);
            GameObject projectileLeft = CreateProjectile(0.25f, dirLeft, 10.0f, Color.green);
            GameObject projectileRight = CreateProjectile(0.25f, dirRight, 10.0f, Color.green);

            GameObject.Destroy(projectile, 0.75f);
            GameObject.Destroy(projectileLeft, 0.75f);
            GameObject.Destroy(projectileRight, 0.75f);
        }
    }
}
