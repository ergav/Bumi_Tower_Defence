using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float      movementSpeed = 5;
    [SerializeField] private int        hp = 3;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (gameManager.prepareState)
        {
            return;
        }
        transform.Translate(Vector2.right * (movementSpeed * Time.deltaTime));
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}