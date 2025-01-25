using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
    [Header("Follower Stats")]
    [SerializeField] private Transform target;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireRate;
    [SerializeField] private int speed;
    [SerializeField] private int followDistance;
    private float cd;
    // Start is called before the first frame update
    void Start()
    {
        cd = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);
        if(distanceToPlayer < followDistance){
            //moves away from player if too close
            transform.position = Vector3.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
        } else if(distanceToPlayer > followDistance+2){
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        } else {
            if(cd > 0){
                cd -= 1*Time.deltaTime;
            }
            else{
                cd = fireRate;
                shoot();
            }
        }
    }

    private void shoot(){
        // Spawn projectile slightly ahead of the enemy
        GameObject projectile = Instantiate(bullet, transform.position + transform.forward, transform.rotation);
    }
    
}

