using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{   
    [Header("Spawner Components")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnRate;
    [SerializeField] private int enemiesToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies(){
        for(int i = 0; i < enemiesToSpawn; i++){
            // Spawn an enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, enemyPrefab.transform.rotation);
            //enemy.GetComponent<EnemyTemplate>().Player = GameObject.FindGameObjectWithTag("Player").transform;
            enemy.GetComponent<EnemyTurret>().target = GameObject.FindGameObjectWithTag("Player").transform;
            yield return new WaitForSeconds(spawnRate);
        }
    }
}