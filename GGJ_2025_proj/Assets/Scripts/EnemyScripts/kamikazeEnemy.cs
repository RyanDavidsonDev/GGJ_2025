using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : MonoBehaviour {

    [Header("Kamikaze Stats")]
    [Tooltip("The movement speed of the enemy.")]
    [SerializeField] private float speed;
    [Tooltip("The radius of the explosion.")]
    [SerializeField] private float explosionRadius;

    [Header("Target Settings")]
    [Tooltip("The target of the enemy.")]
    [SerializeField] private Transform target;

    [Header("Effects")]
    [Tooltip("The explosion effect.")]
    [SerializeField] private GameObject explosionEffect;

    private bool isExploding = false;

    private void Update() {
        if (isExploding || target == null) return; // If the enemy is exploding or has no target stop actions

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget() {
        if (target == null) return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime); // Moves toward the target
        

        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!isExploding && other.transform == target) { // Check if the enemy is exploding and if the collided object is the target
            Explode();
        }
    }

    private void Explode() {
        if (isExploding) return;
        isExploding = true; // Mark the enemy as exploding

        if (explosionEffect != null) {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        if (!TryGetComponent<EnemyTemplate>(out var enemyTemplate)) {
            Destroy(gameObject);
            return;
        }

        int explosionDamage = enemyTemplate.damage;

        var damagable = target.GetComponent<IDamagable>();
        damagable?.TakeDamage(explosionDamage); // Deal damage to the target

        Destroy(gameObject); // Destroy the enemy after exploding
    }
}