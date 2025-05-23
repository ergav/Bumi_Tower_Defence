using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class AttackCollection
    {
        public Transform[] paths;
    }

    [SerializeField] private GameObject[]           enemiesToSpawn;
    [SerializeField] private AttackCollection[]     pathCollection;
    [SerializeField] private float                  spawnRate = 1.0f;

    private float spawnTimer;

    void Start()
    {
        spawnTimer = spawnRate;
    }

    private void Update()
    {
        if(GameManager.instance.prepareState)
        {
            return;
        }

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            GameObject spawnedEnemy = Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)], transform.position, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>().SetPath(pathCollection[Random.Range(0, pathCollection.Length)].paths);
            spawnTimer = spawnRate;
        }
    }
}