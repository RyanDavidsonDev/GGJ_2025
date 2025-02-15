using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Projectile_Count_Stat))]
[RequireComponent(typeof(Fire_Arc_Stat))]
[RequireComponent(typeof(Accuracy_Stat))]
[RequireComponent(typeof(Burst_Stat))]
[RequireComponent(typeof(Delay_Stat))]
[RequireComponent(typeof(Refresh_Stat))]
[RequireComponent(typeof(Charge_Rate_Stat))]
[RequireComponent(typeof(Size_Stat))]
[RequireComponent(typeof(Bubble_Cost_Stat))]

public class Barrel : MonoBehaviour
{
    [Header("Projectile and Muzzle")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject muzzle;
    [Header("Projectile Count")]
    [SerializeField] private Projectile_Count_Stat projectileCount;
    [SerializeField] private Fire_Arc_Stat fireArc;
    [SerializeField] private Accuracy_Stat accuracy;
    [Header("Burst and Fire Rate")]
    [SerializeField] private Burst_Stat burst;
    [SerializeField] private Delay_Stat delay;
    [SerializeField] private Refresh_Stat refresh;
    [Header("Size")]
    [SerializeField] private Size_Stat size;
    [Header("Charge")]
    [SerializeField] private Charge_Rate_Stat chargeRate;
    [SerializeField] private List<Charge_Target> chargeTargets;
    [Header("Cost")]
    [SerializeField] private Bubble_Cost_Stat bubbleCost;
    

    [SerializeField]private float charge;
    private Coroutine chargeCoroutine;
    public float Refresh { get { return refresh.Value; } }
    public float ChargeRate { get { return chargeRate.Value; } }
    public bool IsCharging { get { return chargeCoroutine != null; } }
    private void OnValidate()
    {
        projectileCount = GetComponent<Projectile_Count_Stat>();
        fireArc = GetComponent<Fire_Arc_Stat>();
        accuracy = GetComponent<Accuracy_Stat>();
        burst = GetComponent<Burst_Stat>();
        delay = GetComponent<Delay_Stat>();
        refresh = GetComponent<Refresh_Stat>();
        chargeRate = GetComponent<Charge_Rate_Stat>();
        size = GetComponent<Size_Stat>();

        
    }
    public void StartCharge()
    {
        if (chargeRate.Value > 0)
        {
            if (chargeCoroutine == null)
            {
                chargeCoroutine = StartCoroutine(Charge());
            }
        }
        else
        {
            Fire();
        }
    }
    public void ReleaseCharge()
    {
        if (chargeCoroutine != null)
        {
            StopCoroutine(chargeCoroutine);
            chargeCoroutine = null;
        }
        if (chargeRate.Value > 0)
        {
            Fire(charge);
            charge = 0;
        }
       
    }
    private void Fire(float fireCharge = 0)
    {
        StartCoroutine(FireCoroutine(fireCharge));
        
    }
    private IEnumerator FireCoroutine(float fireCharge)
    {
        if(GameManager.Instance != null){

        GameManager.Instance.ChangeBubbles(Mathf.FloorToInt(-bubbleCost.Value));
        }else {
            Debug.Log("uhoh");
        }

        float timer = delay.Value;
        int shotsFired = 0;
        for (int i = 0; i < chargeTargets.Count; i++)
        {
            float modifier = (fireCharge / 1f) * chargeTargets[i].effectAtMaxCharge;
            chargeTargets[i].target.EphemeralModifier += modifier;

        }
        int count = Mathf.FloorToInt(burst.Value);
        while (shotsFired < count)
        {
            timer += Time.deltaTime;
            if (timer >= delay.Value)
            {
                timer = 0;
                shotsFired++;
                

                for (int i = 0; i < Mathf.FloorToInt( projectileCount.Value); i++)
                {
                    Debug.Log("I " + i);
                    GameObject newProjectile = Instantiate(projectilePrefab);
                    newProjectile.transform.position = muzzle.transform.position;
                    Vector3 rotation = muzzle.transform.rotation.eulerAngles;
                    newProjectile.transform.rotation = Quaternion.Euler(rotation.x, rotation.y + (fireArc.Value * i / Math.Max(projectileCount.Value - 1, 1)) - (.5f * fireArc.Value) + (UnityEngine.Random.Range(-1f, 1f) * accuracy.Value), rotation.z);
                    newProjectile.transform.localScale = new Vector3(size.Value, size.Value, size.Value);
                    newProjectile.SetActive(true);
                    SFXManager.Instance.PlaySound(SFXManager.Instance.BubbleGunFire);
                }
            }
            yield return null;
        }
        for (int i = 0; i < chargeTargets.Count; i++)
        {
            chargeTargets[i].target.ClearEphemeralModifier();

        }
    }
    private IEnumerator Charge()
    {
        while (charge < 1)
        {
            charge = Mathf.Clamp(charge + chargeRate.Value * Time.deltaTime, 0, 1f);
            
            yield return null;
        }
        chargeCoroutine = null;
    }

}
