using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTemplate : MonoBehaviour, IDamagable
{
    [Header("Enemy Stats")]
    [SerializeField] public int health;
    [SerializeField] public int damage;
    [SerializeField] public int bubblesDropped;
    [SerializeField] public GameObject bubblePrefab;
    

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        
        
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

    
}
