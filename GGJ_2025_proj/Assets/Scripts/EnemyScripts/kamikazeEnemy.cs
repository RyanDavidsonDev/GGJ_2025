using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExplosionRing {
    [Tooltip("Radius of the current ring.")]
    public float radius;

    [Tooltip("Damage dealt in this ring.")]
    public int damage;

    [Tooltip("Enables or Disables the stun effect of the outer ring.")]
    public bool stun;

    [Tooltip("Duration of the stun effect.")]
    public float stunDuration;
}

public class KamikazeEnemy : MonoBehaviour {

    [Header("Kamikaze Stats")]
    [Tooltip("The movement speed of the enemy.")]
    [SerializeField] private float speed;
    [Tooltip("The explosion delay after reaching the target (In Seconds).")]
    [SerializeField] private float explosionDelay;

    [Header("Explosion Range and Damage Settings")]
    [Tooltip("The radius of the first explosion ring.")]
    [SerializeField] private List<ExplosionRing> explosionRings = new List<ExplosionRing>();
    [Tooltip("The explosion effects.")]
    [SerializeField] private GameObject explosionEffect;

    [Header("Target Settings")]
    [Tooltip("The target of the enemy.")]
    [SerializeField] private Transform target;

    [Header("Debug Settings")]
    [Tooltip("Enables or Disabled the debug mode for explosion settings.")]
    [SerializeField] private bool debugMode = false;

    private bool isExploding = false;
    private bool isKilledByPlayer = false;
    private float outerRadius;

    private void Start() {
        outerRadius = GetOuterRadius();
    }

    private void Update() {
        if (isExploding || target == null) return; // If the enemy is exploding or has no target stop actions

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget() {
        if (target == null) return;
        Vector3 dir = (target.position - transform.position).normalized;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (dir != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!isExploding && other.transform == target) { // Check if the enemy is exploding and if the collided object is the target
            TriggerExplosion();
        }
    }

    public void TakeDamage(int damage) {
        if (isExploding) return;

        var enemyTemplate = GetComponent<EnemyTemplate>();
        if (enemyTemplate == null) return;

        enemyTemplate.health -= damage;
        if (enemyTemplate.health <= 0) {
            isKilledByPlayer = true;
            TriggerExplosion();
        }
    }

    private void TriggerExplosion() {
        if (isExploding) return;

        isExploding = true;
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay() {
        speed = 0; // Stop movement

        yield return new WaitForSeconds(explosionDelay);

        Explode();
    }

    private void Explode() {
        if (explosionRings.Count == 0) {
            Debug.LogWarning("No explosion rings are configured.");
            return;
        }

        if (explosionEffect != null) {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, outerRadius);
        foreach (var collider in hitColliders) {
            float disToTarget = Vector3.Distance(transform.position, collider.transform.position);

            for (int i = 0; i < explosionRings.Count; i++) {
                if (disToTarget <= GetRingRadius(i)) {
                    HandleRingEffect(collider, explosionRings[i]);
                    break;
                }
            }
        }
        if (isKilledByPlayer) {
            //DropBubbles();
        }

        Destroy(gameObject);
    }

    private void HandleRingEffect(Collider targetCollider, ExplosionRing ring) {
        if (ring.stun) {
            Debug.Log("Target would be stunned for the stun's duration.");
        } else {
            var damagable = targetCollider.GetComponent<IDamagable>();
            damagable?.TakeDamage(ring.damage);
        }
    }

    //private void DropBubbles() {
    //    var enemyTemplate = GetComponent<EnemyTemplate>();
    //    if (enemyTemplate == null) return;

    //    for (int i = 0; i < enemyTemplate.bubblesDropped; i++) {
    //        var bubblePrefab = enemyTemplate.bubblePrefab;
    //        if (bubblePrefab != null) {
    //            Instantiate(bubblePrefab, transform.position, Quaternion.identity);
    //        }
    //    }
    //}


    private float GetOuterRadius() {
        float totalRadius = 0f;
        foreach (var ring in explosionRings) {
            totalRadius += ring.radius;
        }

        return totalRadius;
    }

    private float GetRingRadius(int index) {
        float radius = 0f;
        for (int i = 0; i <= index; i++) {
            radius += explosionRings[i].radius;
        }

        return radius;
    }

    private void OnDrawGizmosSelected() {
        if (!debugMode) return;

        float cumulativeRadius = 0f;

        for (int i = 0; i < explosionRings.Count; i++) {
            cumulativeRadius += explosionRings[i].radius;

            Gizmos.color = i == 0
                ? new Color(1f, 0f, 0f, 0.4f) // Now showing
                : new Color(1f - (i * 0.2f), 0.5f + (i * 0.1f), 0f, 0.3f);

            DrawCircle(transform.position, cumulativeRadius, 16);
        }
    }

    private void DrawCircle(Vector3 center, float radius, int segments) {
        Vector3 previousPoint = Vector3.zero;
        for (int i = 0; i <= segments; i++) {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 currentPoint = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius + center;

            if (i > 0) {
                Gizmos.DrawLine(previousPoint, currentPoint);
            }

            previousPoint = currentPoint;
        }
    }
}