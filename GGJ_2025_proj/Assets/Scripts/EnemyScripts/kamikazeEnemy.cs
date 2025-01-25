using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : MonoBehaviour {

    [Header("Kamikaze Stats")]
    [Tooltip("The movement speed of the enemy.")]
    [SerializeField] private float speed;
    [Tooltip("The radius of the explosion.")]
    [SerializeField] private float explosionRadius;

    [Tooltip("The explosion damage.")]
    [SerializeField] private int explosionDamage;

    [Header("Target Settings")]
    [Tooltip("The target of the enemy.")]
    [SerializeField] private Transform target;

    private bool isExploding = false;

    private void Update() {
        if (isExploding || target == null) return; // If the enemy is exploding or has no target stop actions

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget() {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime); // Moves toward the target
    }

    private void OnTriggerEnter(Collider other) {
        if (!isExploding && other.transform == target) { // Check if the enemy is exploding and if the collided object is the target
            Explode(other);
        }
    }

    private void Explode(Collider other) {
        isExploding = true; // Mark the enemy as exploding

        var damagable = target.GetComponent<IDamagable>();
        if (damagable != null) {
            damagable.TakeDamage(explosionDamage); // Deal damage to the target
        }

        Destroy(gameObject); // Destroy the enemy after exploding
    }
}