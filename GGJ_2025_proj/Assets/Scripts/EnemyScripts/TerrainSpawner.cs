using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{   
    [Header("Spawner Components")]
    [SerializeField] private GameObject[] terrainPrefab;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private float spawnRate;
    [SerializeField] private int terrainToSpawn;
    private Vector3[] randomPositions;
    // Start is called before the first frame update
    void Start()
    {
        randomPositions = createRandomPositionsList();
        StartCoroutine(SpawnTerrain());
    }

    private IEnumerator SpawnTerrain(){
        for(int i = 0; i < terrainToSpawn; i++){

            
            // Spawn an enemy
            GameObject tempTerrain = terrainPrefab[Random.Range(0, terrainPrefab.Length)];
            //Chooses a rotation of either 90 180 or 270 degrees
            int randomRotation = Random.Range(0, 4) * 90;
            Quaternion rotation = Quaternion.Euler(0, randomRotation, 0);

            GameObject terr = Instantiate(tempTerrain, randomPositions[i] , rotation);
            
            
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private Vector3[] createRandomPositionsList()
    {
        Vector3[] randomPositions = new Vector3[terrainToSpawn];


        Vector3 point1 = new Vector3(0, 0, 0);
        Vector3 point2 = new Vector3(0, 0, 0);

        for (int i = 0; i < randomPositions.Length-1; i++)
        {

            point1 = spawnPoints[0].transform.position;
            point2 = spawnPoints[2].transform.position;

            float newX = Random.Range(point1.x, point2.x);
            float newZ = Random.Range(point1.z, point2.z);

            Vector3 newPoint = new Vector3(newX, 0, newZ);

            randomPositions[i] = newPoint;
        }
        return randomPositions;
    }
    
}