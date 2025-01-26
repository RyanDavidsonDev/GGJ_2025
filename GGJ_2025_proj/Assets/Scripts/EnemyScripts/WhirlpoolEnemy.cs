using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlpoolEnemy : MonoBehaviour {
    [Header("Whirlpool Stats")]
    [Tooltip("Radius of the suction effect.")]
    [SerializeField] private float suctionRadius = 10f;
    [Tooltip("Radius where melee damage is applied.")]
    [SerializeField] private float meleeRadius = 1.5f;
    [Tooltip("Strength of the suction effect.")]
    [SerializeField] private float suctionStrength = 5f;
    [Tooltip("Cooldown time between melee attacks (In Seconds).")]
    [SerializeField] private float meleeCooldown = 2f;
    [Tooltip("Texture for the whirlpool effect.")]
    [SerializeField] private Texture2D whirlpoolTexture;
    [Tooltip("Speed of the whirlpool's rotation.")]
    [SerializeField] private float whirlpoolSpeed = 1f;
    [Tooltip("Movement speed of the enemy.")]
    [SerializeField] private float speed = 1f;

    [Header("Debug Settings")]
    [Tooltip("Enable/Disable debug mode.")]
    [SerializeField] private bool debugMode = false;
    [TextArea]
    [SerializeField] private string debugDescription = "Debug Circles:\nGreen: Suction Range\nRed: Melee Range";

    private Transform target;
    private Rigidbody rb;
    private bool isMeleeOnCooldown = false;
    private float meleeCooldownTimer = 0f;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            Debug.LogError("Rigidbody not found.");
            return;
        }

        var enemyTemplate = GetComponent<EnemyTemplate>();
        if (enemyTemplate != null && enemyTemplate.target != null) {
            target = enemyTemplate.target.transform;
        } else {
            Debug.LogError("Target not found in EnemyTemplate.");
        }
    }

    private void FixedUpdate() {
        if (target == null) return;

        float disToTarget = Vector3.Distance(transform.position, target.position);

        if (disToTarget > meleeRadius) {
            MoveTowardsTarget(); // Movement logic from KamikazeEnemy integrated
        }

        if (disToTarget <= suctionRadius && disToTarget > meleeRadius) {
            ApplySuctionEffect();
        }

        if (disToTarget <= meleeRadius && !isMeleeOnCooldown) {
            PerformMeleeAttack();
        }

        if (isMeleeOnCooldown) {
            meleeCooldownTimer -= Time.deltaTime;
            if (meleeCooldownTimer <= 0f) {
                isMeleeOnCooldown = false;
            }
        }
    }
    private void MoveTowardsTarget() {
        if (target == null || rb == null) return;

        Vector3 dir = (target.position - transform.position).normalized;
        rb.MovePosition(transform.position + dir * speed * Time.fixedDeltaTime);
        rb.MoveRotation(Quaternion.LookRotation(dir));
    }

    private void ApplySuctionEffect() {
        if (target == null || rb == null) return;

        Vector3 dirToTarget = target.position - transform.position;
        Vector3 spiralForce = Vector3.Cross(dirToTarget.normalized, Vector3.up) * suctionStrength;

        rb.AddForce((dirToTarget.normalized * suctionStrength + spiralForce) * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void PerformMeleeAttack() {
        isMeleeOnCooldown = true;
        meleeCooldownTimer = meleeCooldown;

        if (TryGetComponent<EnemyTemplate>(out var enemyTemplate)) {
            if (target.TryGetComponent<IDamagable>(out var damagable)) {
                damagable.TakeDamage(enemyTemplate.damage);
            }
        }
    }

    private void OnRenderObj() {
        if (whirlpoolTexture != null) {
            DrawWhirlpool();
        }
    }
    

    private void DrawWhirlpool() {
        Material whirlpoolMaterial = new Material(Shader.Find("Unlit/Texture"));
        whirlpoolMaterial.mainTexture = whirlpoolTexture;
        whirlpoolMaterial.color = new Color(1f, 1f, 1f, 0.6f);

        GL.PushMatrix();
        whirlpoolMaterial.SetPass(0);

        GL.Begin(GL.QUADS);

        float rotation = Time.time * whirlpoolSpeed;

        Vector3 center = transform.position + Vector3.up * 0.1f;
        float size = suctionRadius * 2f;

        Quaternion rotationQuat = Quaternion.Euler(0f, rotation, 0f);
        Vector3 corner1 = rotationQuat * new Vector3(-size / 2, 0, -size / 2);
        Vector3 corner2 = rotationQuat * new Vector3(size / 2, 0, -size / 2);
        Vector3 corner3 = rotationQuat * new Vector3(size / 2, 0, size / 2);
        Vector3 corner4 = rotationQuat * new Vector3(-size / 2, 0, size / 2);

        GL.TexCoord2(0, 0); GL.Vertex(center + corner1);
        GL.TexCoord2(1, 0); GL.Vertex(center + corner2);
        GL.TexCoord2(1, 1); GL.Vertex(center + corner3);
        GL.TexCoord2(0, 1); GL.Vertex(center + corner4);

        GL.End();
        GL.PopMatrix();
    }

    private void OnDrawGizmosSelected() {
        if (!debugMode) return;

        Gizmos.color = Color.green;
        DrawCircle(transform.position, suctionRadius, 16);

        Gizmos.color = Color.red;
        DrawCircle(transform.position, meleeRadius, 16);
    }

    private void DrawCircle(Vector3 center, float radius, int segments) {
        Vector3 previousPoint = center + new Vector3(radius, 0f, 0f);
        for (int i = 1; i <= segments; i++) {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 currentPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }
}