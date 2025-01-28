using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
    [Header("Follower Stats")]
    [SerializeField] public Transform target;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireRate;
    [SerializeField] private int speed;
    [SerializeField] private int followDistance;
    private Rigidbody rb;
    private float cd;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cd = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        rb.MoveRotation(Quaternion.Euler(0,lookRotation.eulerAngles.y,0));

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
                SFXManager.Instance.PlaySound(SFXManager.Instance.BubbleGunFire);
        // Spawn projectile slightly ahead of the enemy
        GameObject projectile = Instantiate(bullet, transform.position + transform.forward, transform.rotation);

        // gives the projectile the damage amount
        EnemyProjectile projectileScript = projectile.GetComponentInChildren<EnemyProjectile>();
        projectile.SetActive(true);
        projectileScript.SetDamage(this.GetComponentInChildren<EnemyTemplate>().damage);
    }
    
}

