using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirer : MonoBehaviour
{

    [SerializeField] public List<FiringController> Guns = new List<FiringController>();

    private SFXManager sFXManager = SFXManager.Instance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BroadcastStartFire()
    {
        SFXManager sFXManager = SFXManager.Instance;
        sFXManager.PlaySound(sFXManager.PlayerHurt);
        foreach(FiringController gun in Guns)
        {
            gun.StartFiring();
        }

        Debug.Log("start firing");
    }

    public void BroadcastStopFire()
    {

        foreach (FiringController gun in Guns)
        {
            gun.StopFiring();
        }
        Debug.Log("stop firing");

    }
}
