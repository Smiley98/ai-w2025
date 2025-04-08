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
