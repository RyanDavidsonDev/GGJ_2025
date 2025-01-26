using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : EnemyProjectile
{
    [SerializeField] private float range;

    protected override void DealDamage(IDamagable damagable, GameObject targetGameObject)
    {
        Collider[] colliders = Physics.OverlapSphere(targetGameObject.transform.position, range, targets);
        foreach (Collider collider in colliders)
        {
            IDamagable subDamageable = collider.GetComponent<IDamagable>();
            if (subDamageable != null)
            {
                subDamageable.TakeDamage(Mathf.FloorToInt(damage));
            }
        }
        MoveProjectile movement = transform.GetComponentInParent<MoveProjectile>();
        Destroy(movement.gameObject);
    }
}
