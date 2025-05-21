using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed = 10;
    private float lifetime = 10;
    private int damage = 1;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(direction * (speed * Time.deltaTime));
    }

    public void OnInstantiate(Vector2 _dir, float _speed = 10, int _damage = 1, float _lifetime = 10)
    {
        direction = _dir;
        speed = _speed;
        damage = _damage;
        lifetime = _lifetime;

        StartCoroutine(LifeSpan());
    }

    IEnumerator LifeSpan()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}