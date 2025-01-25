using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [Header("Turret Stats")]
    [SerializeField] private int range;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject bullet;
    // Start is called before the first frame update
    public float fireRate = 1;
    private float cd;
    void Start()
    {
        cd = fireRate;
    }

    // Update is called once per frame
    void Update()
    {   
        if(cd > 0){
            cd -= 1*Time.deltaTime;
        }
        else{
            cd = fireRate;
            shoot();
        }
    }

    private void shoot(){
        // Shoot at the player
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);
        if(distanceToPlayer <= range){
            // Shoot at the player
            GameObject bulletObject = Instantiate(bullet, transform.position + 2*transform.forward + transform.up, transform.rotation);
            EnemyProjectile projectile = bulletObject.GetComponent<EnemyProjectile>();
            projectile.setDamage(this.GetComponent<EnemyTemplate>().damage);
        } 
    }
}
