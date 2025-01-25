using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class CloakEnemy : MonoBehaviour {
    [Header("Enemy Stats")]
    [Tooltip("Movement Speed for Enemy.")]
    [SerializeField] private float speed;
    [Tooltip("Reveal Range for when the enemy becomes visible by the player.")]
    [SerializeField] private float revealRange;
    [Tooltip("Attack Range for when the enemy can attack the player.")]
    [SerializeField] private float shootingRange;
    [Tooltip("The time the enemy spends aiming before shooting.")]
    [SerializeField] private float aimTime;

    [Tooltip("The bullet object that the enemy will shoot.")]
    [SerializeField] private GameObject bullet;

    [Header("Target Settings")]
    [Tooltip("The target of the enemy.")]
    [SerializeField] private Transform target;

    private Renderer enemyRenderer; // Control visibility
    private bool isVisible = false;
    private bool isAiming = false;
    private float aimTimer = 0f;
    private Vector3 lockedAimPos;

    private void Start() {
        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer != null) {
            enemyRenderer.enabled = false; // Start cloaked
        }
    }

    private void Update() {
        if (target == null) return;

        float disToTarget = Vector3.Distance(transform.position, target.position); // Calculate distance to target


        if (disToTarget <= revealRange) { // Reveals the enemy if the player is within the reveal range
            Reveal();
        } else if (isVisible && disToTarget > revealRange) {
            Cloak(); // Cloaks the enemy if the player is out of the reveal range
        }

        if (isAiming) { // Check if the enemy is aiming
            AimAndShoot(disToTarget);
        } else if (isVisible && disToTarget <= shootingRange && !isAiming) {
            StartAiming(); // If visible and within shooting range start aiming
        } else if (isVisible) {
            MoveTowardsTarget(); // If visible but out of shooting range move towards the target
        }
    }

    private void Reveal() {
        if (!isVisible) {
            isVisible = true;

            if (enemyRenderer != null) {
                enemyRenderer.enabled = true; // Set the enemy as visible
            }
        }
    }

    private void Cloak() {
        if (isVisible) {
            isVisible = false;
            if (enemyRenderer != null) {
                enemyRenderer.enabled = false; // Set the enemy as invisible
            }
        }
    }

    private void MoveTowardsTarget() {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        transform.LookAt(target); // Always look at the target
    }

    private void StartAiming() {
        isAiming = true;
        aimTimer = aimTime; // Reset Aim Timer
        lockedAimPos = target.position;
    }

    private void AimAndShoot(float disToTarget) {
        aimTimer -= Time.deltaTime;

        transform.LookAt(lockedAimPos);

        if (aimTimer <= 0) {
            if (disToTarget <= shootingRange) {
                Shoot();
            }
            isAiming = false;
        }
    }

    private void Shoot() {
        if (bullet != null) {
            GameObject bulletObj = Instantiate(bullet, transform.position + transform.forward, transform.rotation);
        }

        var enemyTemplate = GetComponent<EnemyTemplate>();
        if (enemyTemplate != null) {
            var damagable = target.GetComponent<IDamagable>();
            if (damagable != null) {
                damagable.TakeDamage(enemyTemplate.damage);
            }
        }
    }
}