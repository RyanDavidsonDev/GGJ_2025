using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{   
    [Header("Spawner Components")]
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] spawnPoints;
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
            GameObject tempEnemy = enemyPrefab[Random.Range(0, enemyPrefab.Length)];

            GameObject enemy = Instantiate(tempEnemy, randomPositions[i] , tempEnemy.transform.rotation);
            
            Debug.Log("enemy type: " + tempEnemy.name);
            if(tempEnemy.name == "Turret"){
                enemy.GetComponent<EnemyTurret>().target = GameObject.FindGameObjectWithTag("Player").transform;
            } else if(tempEnemy.name == "Follower"){
                enemy.GetComponent<EnemyFollower>().target = GameObject.FindGameObjectWithTag("Player").transform;
            } else if(tempEnemy.name == "Mimic"){
                enemy.GetComponent<EnemyMimic>().target = GameObject.FindGameObjectWithTag("Player").transform;
            } else{
                Debug.Log("No target found");
            }
            
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
            //int randomSide = Random.Range(0, 4);
            //Debug.Log("random side is " + randomSide);

            //point1 = spawnPoints[randomSide].position;
            //point2 = spawnPoints[(randomSide + 1) % 4].position;//could change this to randPos[].len if we need more corners

            point1 = spawnPoints[0].transform.position;
            point2 = spawnPoints[2].transform.position;

            float newX = Random.Range(point1.x, point2.x);
            float newZ = Random.Range(point1.z, point2.z);

            Vector3 newPoint = new Vector3(newX, 0, newZ);

            Debug.Log("new point is " + newPoint.ToString());
            //Debug.Log(" points are " + point1.ToString() + " and " + point2.ToString());

            //randomPositions[i] = Vector3.Lerp(point1, point2, Random.Range(0.0f, 1.0f));
            randomPositions[i] = newPoint;
        }
        return randomPositions;
    }
    
}