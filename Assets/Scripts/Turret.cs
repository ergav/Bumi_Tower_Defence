using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform      currentTarget;
    [SerializeField] private Transform      muzzle;

    [SerializeField] private float          rotationSpeed = 1.0f;
    [SerializeField] private float          detectionRadius = 50.0f;
    [SerializeField] private float          rateOfFire = 1.0f;

    [SerializeField] private GameObject     projectilePrefab;

    private float shootTimer;

    void Start()
    {
        shootTimer = rateOfFire;
    }

    void Update()
    {
        RotateTowardsTarget();
    }

    void RotateTowardsTarget()
    {
        if (currentTarget == null)
            return;

        float angle = Mathf.Atan2(transform.position.y - currentTarget.position.y, transform.position.x - currentTarget.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Vector2 forward = transform.TransformDirection(Vector2.left);
        Vector2 toOther = Vector3.Normalize(currentTarget.position - transform.position);

        if (Vector3.Dot(forward, toOther) > 0.98f)
        {
            shootTimer -= Time.deltaTime;

            if (shootTimer <= 0)
            {
                FireProjectile(toOther);
                shootTimer = rateOfFire;
            }
        }
        else 
        {
            shootTimer = rateOfFire;
        }
    }

    void FireProjectile(Vector2 dir)
    {
        Debug.Log("FIRE!");
        Projectile firedProjectile = Instantiate(projectilePrefab.GetComponent<Projectile>(), muzzle.position, Quaternion.identity);
        firedProjectile.OnInstantiate(dir);
    }
}