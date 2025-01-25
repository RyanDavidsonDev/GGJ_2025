using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
    [Header("Follower Stats")]
    [SerializeField] private Transform target;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int fireRate;
    [SerializeField] private int speed;
    [SerializeField] private int followDistance;
    private int cd;
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
                cd--;
            }
            else{
                cd = fireRate;
                shoot();
            }
        }
    }

    private void shoot(){
        // Shoot at the player
        GameObject bulletObject = Instantiate(bullet, transform.position, transform.rotation);
    }
}
