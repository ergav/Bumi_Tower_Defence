using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float      movementSpeed = 5;
    [SerializeField] private int        hp = 3;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector2.right * (movementSpeed * Time.deltaTime));
    }

    void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}