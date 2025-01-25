using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Projectile_Count_Stat))]
[RequireComponent(typeof(Fire_Arc_Stat))]
[RequireComponent(typeof(Accuracy_Stat))]
[RequireComponent(typeof(Burst_Stat))]
[RequireComponent(typeof(Delay_Stat))]
[RequireComponent(typeof(Refresh_Stat))]
[RequireComponent(typeof(Max_Charge_Stat))]
[RequireComponent(typeof(Charge_Rate_Stat))]
[RequireComponent(typeof(Charge_Multiplier_Stat))]
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
    [Header("Charge")]
    [SerializeField] Max_Charge_Stat maxCharge;
    [SerializeField] private Charge_Rate_Stat chargeRate;
    [SerializeField] private Charge_Multiplier_Stat chargeMultiplier;
    [SerializeField] private List<Stat> chargeTargets;

    private void OnValidate()
    {
        projectileCount = GetComponent<Projectile_Count_Stat>();
        fireArc = GetComponent<Fire_Arc_Stat>();
        accuracy = GetComponent<Accuracy_Stat>();
        burst = GetComponent<Burst_Stat>();
        delay = GetComponent<Delay_Stat>();
        refresh = GetComponent<Refresh_Stat>();
        maxCharge = GetComponent<Max_Charge_Stat>();
        chargeRate = GetComponent<Charge_Rate_Stat>();
        chargeMultiplier = GetComponent<Charge_Multiplier_Stat>();
    }
}
