using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform      currentTarget;
    [SerializeField] private Transform      turretBody;
    [SerializeField] private Transform      muzzle;

    [SerializeField] private float          rotationSpeed = 1.0f;
    [SerializeField] private float          detectionRadius = 50.0f;
    [SerializeField] private float          loseTargetRange = 60.0f;
    [SerializeField] private float          rateOfFire = 1.0f;
    [SerializeField] private int            damagePower = 1;
    [SerializeField] private float          projectileSpeed = 20;
    
    [Range(0, 1)]
    [SerializeField] private float          aimDotProduct = 0.98f;

    [SerializeField] private GameObject     projectilePrefab;

    [SerializeField] private LayerMask      enemyLayerMask;

    private float shootTimer;

    public Priority priority = Priority.prioritizeClosest;

    public enum Priority { prioritizeClosest, prioritizeFurthest }

    void Start()
    {
        shootTimer = rateOfFire;
    }

    private float detectionTimer = 0.5f;
    void Update()
    {
        if(GameManager.instance.prepareState)
        {
            return;
        }

        RotateTowardsTarget();

        detectionTimer -= Time.deltaTime;

        if (detectionTimer <= 0)
        {
            detectionTimer = 0.5f;

            switch (priority)
            {
                case Priority.prioritizeClosest:
                    currentTarget = GetNearestTarget();
                    break;
                case Priority.prioritizeFurthest:
                    currentTarget = GetFurthestTarget();
                    break;
            }
        }

        if (currentTarget != null)
        {
            if (Vector2.Distance(transform.position, currentTarget.position) > loseTargetRange)
            { 
                currentTarget = null;
            }
        }
    }

    Transform GetNearestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayerMask);

        if (colliders.Length == 0)
        {
            return null;
        }

        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            float dist = Vector3.Distance(collider.transform.position, transform.position);
            if (dist < minDist)
            {
                closest = collider.transform;
                minDist = dist;
            }
        }

        return closest;
    }

    Transform GetFurthestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayerMask);

        if (colliders.Length == 0)
        {
            return null;
        }

        Transform furthest = null;
        float maxDist = 0;

        foreach (Collider2D collider in colliders)
        {
            float dist = Vector3.Distance(collider.transform.position, transform.position);
            if (dist > maxDist)
            {
                furthest = collider.transform;
                maxDist = dist;
            }
        }

        return furthest;
    }

    void RotateTowardsTarget()
    {
        if (currentTarget == null)
        {
            return;
        }

        float angle = Mathf.Atan2(turretBody.position.y - currentTarget.position.y, turretBody.position.x - currentTarget.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        turretBody.rotation = Quaternion.RotateTowards(turretBody.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Vector2 forward = turretBody.TransformDirection(Vector2.left);
        Vector2 toOther = Vector3.Normalize(currentTarget.position - turretBody.position);

        if (Vector3.Dot(forward, toOther) > 0.98f)
        {
            shootTimer -= Time.deltaTime;

            if (shootTimer <= 0)
            {
                FireProjectile(toOther);
                shootTimer = rateOfFire;
            }
        }
    }

    void FireProjectile(Vector2 dir)
    {
        Projectile firedProjectile = Instantiate(projectilePrefab.GetComponent<Projectile>(), muzzle.position, Quaternion.identity);
        firedProjectile.OnInstantiate(dir, damagePower, projectileSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}