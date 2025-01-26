using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloakEnemy : MonoBehaviour {
    [Header("Enemy Stats")]
    [Tooltip("Movement Speed for Enemy.")]
    [SerializeField] private float speed = 1f;
    [Tooltip("Reveal Range for when the enemy becomes visible to the player.")]
    [SerializeField] private float revealRange = 5f;
    [Tooltip("Attack Range for when the enemy can attack the player.")]
    [SerializeField] private float shootingRange = 2f;
    [Tooltip("The time the enemy spends aiming before shooting. (In Seconds)")]
    [SerializeField] private float aimTime = 2f;

    [Tooltip("The bullet object that the enemy will shoot.")]
    [SerializeField] private GameObject bullet;

    [Header("Target Settings")]
    [Tooltip("The target of the enemy.")]
    [SerializeField] private Transform target;

    [Header("Cloak Effect Settings")]
    [Tooltip("Minimum alpha value when invisible.")]
    [SerializeField, Range(0f, 1f)] private float invisibleAlpha = 0.2f;

    [Header("Debug Settings")]
    [Tooltip("Enable/Disable debug mode.")]
    [SerializeField] private bool debugMode = false;
    [TextArea]
    [SerializeField] private string debugDescription = "Debug Circles:\nGreen: Reveal Range\nRed: Shooting Range";

    private Renderer[] enemyRenderers;
    private Material[] originalMaterials;
    private bool isVisible = false;
    private bool isAiming = false;
    private float aimTimer = 0f;
    private Vector3 lockedAimPos;

    private void Start() {
        enemyRenderers = GetComponentsInChildren<Renderer>();
        StoreOriginalMaterials();
        ApplyInvisibilityEffect(false); // Start invisible
    }

    private void Update() {
        if (target == null) return;

        float disToTarget = Vector3.Distance(transform.position, target.position);

        if (disToTarget <= revealRange) {
            Reveal();
        } else if (isVisible && disToTarget > revealRange) {
            Cloak();
        }

        if (isAiming) {
            HandleAimingAndShooting();
        } else if (isVisible) {
            if (disToTarget <= shootingRange) {
                StartAiming();
            } else {
                MoveTowardsTarget();
            }
        }
    }

    private void Reveal() {
        if (!isVisible) {
            isVisible = true;
            ApplyInvisibilityEffect(false);
        }
    }

    private void Cloak() {
        if (isVisible) {
            isVisible = false;
            ApplyInvisibilityEffect(true);
        }
    }

    private void MoveTowardsTarget() {
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (dir != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    private void StartAiming() {
        isAiming = true;
        aimTimer = aimTime;
        lockedAimPos = target.position;
    }

    private void HandleAimingAndShooting() {
        aimTimer -= Time.deltaTime;

        transform.LookAt(lockedAimPos);

        if (aimTimer <= 0) {
            Shoot();
            isAiming = false;
        }
    }

    private void Shoot() {
        if (bullet != null) {
            Instantiate(bullet, transform.position + transform.forward, transform.rotation);
        }

        if (TryGetComponent<EnemyTemplate>(out var enemyTemplate)) {
            if (target.TryGetComponent<IDamagable>(out var damagable)) {
                damagable.TakeDamage(enemyTemplate.damage);
            }
        }
    }

    private void StoreOriginalMaterials() {
        originalMaterials = new Material[enemyRenderers.Length];
        for (int i = 0; i < enemyRenderers.Length; i++) {
            originalMaterials[i] = enemyRenderers[i].material;
        }
    }

    private void ApplyInvisibilityEffect(bool isInvisible) {
        foreach (var material in originalMaterials) {
            if (material.HasProperty("_Color")) {
                Color color = material.color;
                color.a = isInvisible ? invisibleAlpha : 1f; // Set alpha
                material.color = color;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        if (!debugMode) return;

        Gizmos.color = Color.green;
        DrawCircle(transform.position, revealRange, 16);

        Gizmos.color = Color.red;
        DrawCircle(transform.position, shootingRange, 16);
    }

    private void DrawCircle(Vector3 center, float radius, int segments) {
        Vector3 previousPoint = center + new Vector3(radius, 0, 0);
        for (int i = 1; i <= segments; i++) {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 currentPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }
}