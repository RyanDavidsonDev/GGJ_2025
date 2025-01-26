using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    

    public float damage = 1;
    void Start()
    {
        
    }

    // Update is called once per frame


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
