using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Projectile Attributes")]
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifeSpan = 5f;

    public float damage = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        projectileLifeSpan -= Time.deltaTime;
        this.transform.position += this.transform.forward * projectileSpeed * Time.deltaTime;
        if(projectileLifeSpan <= 0){
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {   
        if(other.gameObject.tag == "Player"){
            Destroy(this.gameObject);
        }
    }

    public void setDamage(float damage){
        this.damage = damage;
    }
}
