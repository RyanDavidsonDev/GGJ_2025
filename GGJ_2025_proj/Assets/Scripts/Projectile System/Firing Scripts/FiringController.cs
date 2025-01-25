using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FiringController : MonoBehaviour
{
    public bool fire = false;
    [SerializeField] private FireMode fireMode;
    [SerializeField] private List<Barrel> barrelList = new List<Barrel>();
    [SerializeField]private List<float> timers = new List<float>();
    private Coroutine fireCoroutine;
    private float timer;
    public void StartFiring()
    {
       
        switch (fireMode) {
            case FireMode.Alternating:
                fireCoroutine = StartCoroutine(FireAlternating());
                break;

            case FireMode.Simultanious:
                fireCoroutine = StartCoroutine(FireSimultanious());
                break;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (fire && fireCoroutine == null)
        {
            StartFiring();
        }
        else if(!fire && fireCoroutine != null)
        {
            StopFiring();
        }
    }
    public void StopFiring()
    {
        StopCoroutine(fireCoroutine);
        fireCoroutine = null;
        for (int i = 0; i < barrelList.Count; i++)
        {
            barrelList[i].ReleaseCharge();
        }
    }
    private IEnumerator FireAlternating()
    {
        int currentBarrel = 0;
        while (true)
        {
            timer += Time.deltaTime;
            
            if (timer >= barrelList[currentBarrel].Refresh && !barrelList[currentBarrel].IsCharging)
            {
                barrelList[currentBarrel].StartCharge();
                timer = 0;
                currentBarrel = Mathf.FloorToInt(Mathf.Repeat(currentBarrel + 1, barrelList.Count));
            }

            yield return null;
        }
        
    }
    private IEnumerator FireSimultanious()
    {
        for(int i = 0; i < timers.Count; i++)
        {
            timers[i] += timer;
            timer = 0;
        }
        while (true)
        {
            for (int i = 0; i < barrelList.Count; i++)
            {
                timers[i] += Time.deltaTime;
                if (timers[i] >= barrelList[i].Refresh && !barrelList[i].IsCharging)
                {
                    barrelList[i].StartCharge();
                    timers[i] = 0;
                }
            }
            yield return null;
        }
    }
    private void OnValidate()
    {
        timers.Clear();
        for (int i = 0; i < barrelList.Count; i++)
        {
            timers.Add(0);
        }
    }
}


public enum FireMode
{
    Alternating,
    Simultanious
}
