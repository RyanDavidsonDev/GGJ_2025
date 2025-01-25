using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMimic : MonoBehaviour
{

    [Header("Mimic properties")]
    [SerializeField] private Transform Target;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private int ShootRange;
    [SerializeField] private int TransformRange;
    [SerializeField] private float FireRate;
    [SerializeField] private Texture3D newTexture;
    [SerializeField] private Mesh newMesh;
    // Start is called before the first frame update
    //cooldown variable

    private float cd;
    private bool isShooting = false;
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        cd = FireRate;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(Target.position, transform.position);
        if(distanceToPlayer <= TransformRange){
            //mimic changes form if the player gets close enough
            changeForm();
        }
        if(isShooting){
            if(cd > 0){
                cd -= 1*Time.deltaTime;
            }
            else{
                cd = FireRate;
                shoot(distanceToPlayer);
            }
        }
    }

    private void shoot(float distanceToPlayer){
        int damage = this.GetComponent<EnemyTemplate>().damage;
        // Shoot at the player
        if(distanceToPlayer <= ShootRange){
            // creates the bullet and declares its damage
            GameObject bulletObject = Instantiate(Bullet, transform.position + transform.forward, transform.rotation);
            EnemyProjectile bullet = bulletObject.GetComponent<EnemyProjectile>();
            bullet.setDamage(damage);
        }
    }

    private void changeForm(){
        //transforms into shooting state
        isShooting = true;
        //Renderer renderer = GetComponent<Renderer>();
        //if (renderer != null && newTexture != null)
        //{
            // Set the main texture of the material.
            //renderer.material.mainTexture = newTexture;
        gameObject.GetComponent<MeshFilter>().mesh = newMesh;
        //}
        //else
        //{
        //    Debug.LogWarning("Renderer or texture is missing!");
        //}
    }
}
