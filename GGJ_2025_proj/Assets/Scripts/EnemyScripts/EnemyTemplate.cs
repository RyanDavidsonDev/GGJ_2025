using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTemplate : MonoBehaviour, IDamagable
{
    [Header("Enemy Stats")]
    [SerializeField] public int health;
    [SerializeField] public int damage;
    [SerializeField] private int bubblesDropped;
    
    public Transform Player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        this.gameObject.transform.LookAt(Player);
        
        //checks to see if the enemy is being damaged using damager tag


    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0){
            Die();
        }
    }

    public void Die(){
        // Drop bubbles
        for(int i = 0; i < bubblesDropped; i++){
            // Drop a bubble
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "EnemyDamager"){
            // Take damage, must be changed to interact with player projectiles
            EnemyProjectile projectile = other.gameObject.GetComponent<EnemyProjectile>();
            TakeDamage((int)projectile.damage);
        }
    }
}
