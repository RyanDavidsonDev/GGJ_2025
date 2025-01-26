using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [Header("Turret Stats")]
    [SerializeField] private int range;
    [SerializeField] public Transform target;
    [SerializeField] private GameObject bullet;

    private Rigidbody rb;
    // Start is called before the first frame update
    public float fireRate = 1;
    private float cd;
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
        if(cd > 0){
            cd -= 1*Time.deltaTime;
            SFXManager.Instance.PlaySound(SFXManager.Instance.BubbleGunFire);
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
            EnemyProjectile projectile = bulletObject.GetComponentInChildren<EnemyProjectile>();
            projectile.SetDamage(this.GetComponentInChildren<EnemyTemplate>().damage);
            bulletObject.SetActive(true);
        } 
    }
}
