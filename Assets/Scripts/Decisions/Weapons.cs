using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public abstract class Weapon
{
    public GameObject weaponPrefab;
    public GameObject owner;
    public float timeCurrent = 0.0f;
    public float timeMax = 0.0f; // <-- how long we wait in-between shots (ie 0.1 for machine gun, 1.0 for sniper)
    public float speed = 1.0f;

    public abstract void Shoot(Vector3 direction);
}

public class Rifle : Weapon
{
    public override void Shoot(Vector3 direction)
    {
        timeCurrent += Time.deltaTime;
        if (timeCurrent >= timeMax)
        {
            timeCurrent = 0.0f;

            GameObject projectile = GameObject.Instantiate(weaponPrefab);
            projectile.transform.localScale *= 0.25f;
            float bulletRadius = projectile.transform.localScale.x * 0.5f;
            float ownerRadius = owner.transform.localScale.x * 0.5f;

            projectile.transform.position = owner.transform.position + direction * (ownerRadius + bulletRadius);
            projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * 20.0f;
            projectile.GetComponent<SpriteRenderer>().color = Color.red;
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

            GameObject projectile = GameObject.Instantiate(weaponPrefab);
            GameObject projectileLeft = GameObject.Instantiate(weaponPrefab);
            GameObject projectileRight = GameObject.Instantiate(weaponPrefab);

            projectile.transform.localScale *= 0.25f;
            projectileLeft.transform.localScale *= 0.25f;
            projectileRight.transform.localScale *= 0.25f;

            float bulletRadius = projectile.transform.localScale.x * 0.5f;
            float ownerRadius = owner.transform.localScale.x * 0.5f;

            Vector3 dirLeft = Quaternion.Euler(0.0f, 0.0f, 20.0f) * direction;
            Vector3 dirRight = Quaternion.Euler(0.0f, 0.0f, -20.0f) * direction;

            projectile.transform.position = owner.transform.position + direction * (ownerRadius + bulletRadius);
            projectileLeft.transform.position = owner.transform.position + dirLeft * (ownerRadius + bulletRadius);
            projectileRight.transform.position = owner.transform.position + dirRight * (ownerRadius + bulletRadius);

            projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * 20.0f;
            projectileLeft.GetComponent<Rigidbody2D>().linearVelocity = dirLeft * 20.0f;
            projectileRight.GetComponent<Rigidbody2D>().linearVelocity = dirRight * 20.0f;

            projectile.GetComponent<SpriteRenderer>().color = Color.red;
            projectileLeft.GetComponent<SpriteRenderer>().color = Color.red;
            projectileRight.GetComponent<SpriteRenderer>().color = Color.red;

            GameObject.Destroy(projectile, 1.0f);
            GameObject.Destroy(projectileLeft, 1.0f);
            GameObject.Destroy(projectileRight, 1.0f);
        }
    }
}
