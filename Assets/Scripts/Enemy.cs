using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float      movementSpeed = 5;
    [SerializeField] private int        hp = 3;

    [SerializeField] 
    private Transform[]                 wayPoints;
    private int                         currentWayPoint;

    void Update()
    {
        if (GameManager.instance.prepareState)
        {
            return;
        }

        Vector2 dirToWayPoint = Vector3.Normalize(wayPoints[currentWayPoint].position - transform.position);

        transform.Translate(dirToWayPoint * (movementSpeed * Time.deltaTime));

        if (Vector2.Distance(transform.position, wayPoints[currentWayPoint].position) < 0.1f)
        {
            currentWayPoint++;
            if (currentWayPoint >= wayPoints.Length)
            {
                GameManager.instance.MissedEnemiesIncrement();
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetPath(Transform[] path)
    {
        wayPoints = path;
    }
}