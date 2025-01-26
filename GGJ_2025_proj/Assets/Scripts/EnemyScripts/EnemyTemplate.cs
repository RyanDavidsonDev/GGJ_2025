using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTemplate : MonoBehaviour, IDamagable
{
    [Header("Enemy Stats")]
    [SerializeField] public int health;
    [SerializeField] public int damage;
    [Tooltip("This dictates a range for the number of bubbles to drop, X is min, Y is max")]
    [SerializeField] public Vector2 bubblesDropped;
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

    private void DropBubbles(int dropped)
    {
        for(int i = 0; i< dropped; i++)
        {
            Instantiate(bubblePrefab, transform.position, Quaternion.identity);
        }
    }

    public void Die(){
        // Drop bubbles
        DropBubbles(Mathf.RoundToInt(Random.Range(bubblesDropped.x, bubblesDropped.y)));
        Destroy(this.gameObject);
    }

    
}
