using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


[System.Serializable]
public class SpawnPointInfo {
    [Tooltip("The spawn Point.")]
    public Transform spawnPoint;
    [Tooltip("Allowed enemy prefabs to spawn at this point.")]
    public GameObject[] allowedEnemies;
    [Tooltip("The spawn amount for this point.")]
    public int spawnAmount = 1;
    [Tooltip("If true only one enemy will spawn at this point.")]
    public bool restrictSpawnToOne = false;
    [Tooltip("The radius of the spawn area.")]
    public float spawnAreaRadius = 1f;
    [Tooltip("The spawn rate for this specific point.")]
    public float spawnRate = 2f;

    [HideInInspector] public List<GameObject> activeEnemies = new List<GameObject>();
    [HideInInspector] public float spawnTimer;
}

public class UnifiedSpawner : MonoBehaviour {
    [Header("Spanwer Settings")]
    [Tooltip("The spawn point configurations for each one.")]
    [SerializeField] private List<SpawnPointInfo> spawnPoints = new List<SpawnPointInfo>();

    [Header("Debug Settings")]
    [Tooltip("Enable/Disable debug mode.")]
    [SerializeField] private bool debugMode = false;
    [Tooltip("Color of the spawn radius while in debug mode.")]
    [SerializeField] private Color debugColor = new Color(0f, 1f, 0f, 0.2f);

    private GameObject target;

    private float debugCylinderHeight = 0.05f;

    private void Start() {
        Debug.Log("Starting Spawner...");
        target = GameObject.FindGameObjectWithTag("Player");
        if (target == null) {
            Debug.LogError("Player target not found.");
            return;
        }
        foreach (var spawnPoint in spawnPoints) {
            Debug.Log($"Starting spawner for: {spawnPoint.spawnPoint.name}");
            spawnPoint.spawnTimer = spawnPoint.spawnRate;
        }
    }

    private void FixedUpdate() {
        foreach (var spawnPoint in spawnPoints) {
            HandleSpawnPoint(spawnPoint);
        }
    }

    private void HandleSpawnPoint(SpawnPointInfo spawnPointInfo) {
        if (spawnPointInfo.restrictSpawnToOne && spawnPointInfo.activeEnemies.Count > 0) return;

        spawnPointInfo.spawnTimer -= Time.fixedDeltaTime;
        if (spawnPointInfo.spawnTimer <= 0) {
            for (int i = 0; i < spawnPointInfo.spawnAmount; i++) {
                SpawnEnemyAtPoint(spawnPointInfo);
            }
            spawnPointInfo.spawnTimer = spawnPointInfo.spawnRate;
        }
    }

    private void SpawnEnemyAtPoint(SpawnPointInfo spawnPointInfo) {
        if (spawnPointInfo.allowedEnemies.Length == 0) return;
        int randIdx = Random.Range(0, spawnPointInfo.allowedEnemies.Length);
        GameObject ePrefab = spawnPointInfo.allowedEnemies[randIdx];

        Vector3 randPos = spawnPointInfo.spawnPoint.position + Random.insideUnitSphere * spawnPointInfo.spawnAreaRadius;
        randPos.y = spawnPointInfo.spawnPoint.position.y;

        GameObject enemy = Instantiate(ePrefab, randPos, Quaternion.identity);
        spawnPointInfo.activeEnemies.Add(enemy);

        var enemyTemplate = enemy.GetComponent<EnemyTemplate>();
        if (enemyTemplate != null) {
            enemyTemplate.target = target;
        }
    }

    public void OnDestroyCallback(GameObject enemy, SpawnPointInfo spawnPointInfo) {
        if (spawnPointInfo.activeEnemies.Contains(enemy)) {
            spawnPointInfo.activeEnemies.Remove(enemy);
        }
    }

    private void OnDrawGizmosSelected() {
        if (!debugMode) return;

        foreach (var spawnPoint in spawnPoints) {
            Gizmos.color = debugColor;
            DrawCylinder(spawnPoint.spawnPoint.position, spawnPoint.spawnAreaRadius, debugCylinderHeight, 16);
        }
    }

    private void DrawCylinder(Vector3 center, float radius, float height, int segments) {
        float angleStep = 360f / segments;

        for (int i = 0; i < 2; i++) {
            float y = center.y + (i == 0 ? height / 2 : -height / 2);
            Vector3 previousPoint = center + new Vector3(radius, y - center.y, 0);

            for (int j = 1; j <= segments; j++) {
                float angle = j * angleStep * Mathf.Deg2Rad;
                Vector3 currentPoint = center + new Vector3(Mathf.Cos(angle) * radius, y - center.y, Mathf.Sin(angle) * radius);

                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }
        }

        for (int j = 0; j < segments; j++) {
            float angle = j * angleStep * Mathf.Deg2Rad;
            Vector3 bottomPoint = center + new Vector3(Mathf.Cos(angle) * radius, -height / 2, Mathf.Sin(angle) * radius);
            Vector3 topPoint = center + new Vector3(Mathf.Cos(angle) * radius, height / 2, Mathf.Sin(angle) * radius);

            Gizmos.DrawLine(bottomPoint, topPoint);
        }
    }
}