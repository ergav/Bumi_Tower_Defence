using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform currentTarget;

    [SerializeField] private float rotationSpeed = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        RotateTowardsTarget();
    }

    void RotateTowardsTarget()
    {
        if (currentTarget == null)
            return;

        float angle = Mathf.Atan2(currentTarget.position.y - transform.position.y, currentTarget.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}