using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    [Header("Projectile Attributes")]
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifeSpan = 5f;
    private void Update()
    {
        Move();

    }
    public virtual void Move()
    {
        
        projectileLifeSpan -= Time.deltaTime;
        this.transform.position += this.transform.forward * projectileSpeed * Time.deltaTime;
        if (projectileLifeSpan <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
