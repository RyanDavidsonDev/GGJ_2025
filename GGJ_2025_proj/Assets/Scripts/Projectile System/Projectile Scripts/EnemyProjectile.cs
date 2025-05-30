using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    

    [SerializeField] protected float damage = 1;
    [SerializeField] public LayerMask targets;

    void Start()
    {
        
    }

    // Update is called once per frame


    private void OnTriggerEnter(Collider other)
    {
        if ((targets & (1 << other.gameObject.layer)) != 0)
        {
            IDamagable damagable = other.GetComponent<IDamagable>();
            if (damagable != null)
            {
                DealDamage(damagable, other.gameObject);
            }
        }
    }
    protected virtual void DealDamage(IDamagable damagable, GameObject targetGameObject)
    {
        damagable.TakeDamage(Mathf.FloorToInt(damage));
        MoveProjectile movement = transform.GetComponentInParent<MoveProjectile>();
        Destroy(movement.gameObject);
    }

    public void SetDamage(float damage){
        this.damage = damage;
    }
}
