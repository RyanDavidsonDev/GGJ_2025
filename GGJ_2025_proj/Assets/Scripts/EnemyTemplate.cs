using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTemplate : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int health;
    [SerializeField] private int damage;
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
    }
}
