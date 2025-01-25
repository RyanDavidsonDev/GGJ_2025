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
        if (isExploding || target == null) return;

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget() {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (!isExploding && other.transform == target) {
            Explode(other);
        }
    }

    private void Explode(Collider other) {
        isExploding = true;

        var damagable = target.GetComponent<IDamagable>();
        if (damagable != null) {
            damagable.TakeDamage(explosionDamage);
        }

        Destroy(gameObject);
    }
}