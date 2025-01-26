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
    private Vector3[] randomPositions;
    // Start is called before the first frame update
    void Start()
    {
        randomPositions = createRandomPositionsList();
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies(){
        for(int i = 0; i < enemiesToSpawn; i++){

            
            // Spawn an enemy

            GameObject enemy = Instantiate(enemyPrefab, randomPositions[i] , enemyPrefab.transform.rotation);
            //enemy.GetComponent<EnemyTemplate>().Player = GameObject.FindGameObjectWithTag("Player").transform;
            enemy.GetComponent<EnemyTurret>().target = GameObject.FindGameObjectWithTag("Player").transform;
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private Vector3[] createRandomPositionsList()
    {
        Vector3[] randomPositions = new Vector3[enemiesToSpawn];


        Vector3 point1 = new Vector3(0, 0, 0);
        Vector3 point2 = new Vector3(0, 0, 0);

        for (int i = 0; i < randomPositions.Length-1; i++)
        {
            int randomSide = Random.Range(0, 4);
        Debug.Log("random side is " + randomSide);

            point1 = spawnPoints[randomSide].position;
            point2 = spawnPoints[(randomSide + 1) % 4].position;//could change this to randPos[].len if we need more corners


            Debug.Log(" points are " + point1.ToString() + " and " + point2.ToString());

            randomPositions[i] = Vector3.Lerp(point1, point2, Random.Range(0.0f, 1.0f));
        }
        return randomPositions;
    }
    
}