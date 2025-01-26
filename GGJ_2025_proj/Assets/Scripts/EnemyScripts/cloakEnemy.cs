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

    [Header("Cloak Effect Settings")]
    [Tooltip("Minimum alpha value when invisible.")]
    [SerializeField, Range(0f, 1f)] private float invisibleAlpha = 0.2f;

    [Header("Debug Settings")]
    [Tooltip("Enable/Disable debug mode.")]
    [SerializeField] private bool debugMode = false;
    [TextArea]
    [SerializeField] private string debugDescription = "Debug Circles:\nGreen: Reveal Range\nRed: Shooting Range";

    private Rigidbody rb;
    private Renderer[] enemyRenderers;
    private Material[] originalMaterials;
    private bool isVisible = false;
    private bool isAiming = false;
    private float aimTimer = 0f;
    private Vector3 lockedAimPos;
    private Transform target;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            Debug.LogError("Rigidbody not found.");
            return;
        }

        InitMats();

        var enemyTemplate = GetComponent<EnemyTemplate>();
        if (enemyTemplate != null && enemyTemplate.target != null) {
            target = enemyTemplate.target.transform;
        } else {
            Debug.LogError("Target not found in EnemyTemplate.");
        }
    }

    private void Update() {
        if (target == null) return;

        float disToTarget = Vector3.Distance(transform.position, target.position);

        Debug.Log($"Distance to target: {disToTarget}");

        HandleVisibility(disToTarget);
        HandleBehaviour(disToTarget);
    }

    private void FixedUpdate() {
        if (!isAiming && target != null) {
            MoveTowardsTarget();
        }
    }

    private void HandleVisibility(float disToTarget) {
        if (disToTarget <= revealRange) {
            SetVisibility(true);
        } else if (isVisible) {
            SetVisibility(false);
        }
    }

    private void HandleBehaviour(float disToTarget) {
        if (isAiming) {
            HandleAimingAndShooting();
        } else if (isVisible) {
            if (disToTarget <= shootingRange) {
                StartAiming();
            }
        }
    }

    private void SetVisibility(bool visible) {
        if (isVisible == visible) return;

        Debug.Log($"Visibility changed to {visible} for {gameObject.name}");
        isVisible = visible;
        ApplyInvisibilityEffect(!visible);
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
        if (bullet == null) return;

        GameObject projectile = Instantiate(bullet, transform.position + transform.forward, transform.rotation);
        if (projectile.TryGetComponent<EnemyProjectile>(out var projectileScript)) {
            //projectileScript.setDamage(GetComponent<EnemyTemplate>().damage);
        }
    }

    private void MoveTowardsTarget() {
        Vector3 dir = (target.position - transform.position).normalized;
        rb.MovePosition(transform.position + dir * speed * Time.fixedDeltaTime);
        rb.MoveRotation(Quaternion.LookRotation(dir));
    }
    private void InitMats() {
        enemyRenderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[enemyRenderers.Length];
        for (int i = 0; i < enemyRenderers.Length; i++) {
            originalMaterials[i] = enemyRenderers[i].material;
        }
        ApplyInvisibilityEffect(true);
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
        DrawDebugCircle(transform.position, revealRange, 16);

        Gizmos.color = Color.red;
        DrawDebugCircle(transform.position, shootingRange, 16);
    }

    private void DrawDebugCircle(Vector3 center, float radius, int segments) {
        Vector3 previousPoint = center + new Vector3(radius, 0, 0);
        for (int i = 1; i <= segments; i++) {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 currentPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }
}