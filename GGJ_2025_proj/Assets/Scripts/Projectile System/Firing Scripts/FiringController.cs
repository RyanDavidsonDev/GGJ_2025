using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FiringController : MonoBehaviour
{
    [SerializeField] private FireMode fireMode;
    [SerializeField] private List<Barrel> barrelList = new List<Barrel>();
    private List<float> timers = new List<float>();
    private Coroutine fireCoroutine;
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
    public void StopFiring()
    {
        StopCoroutine(fireCoroutine);
        for (int i = 0; i < barrelList.Count; i++)
        {
            barrelList[i].ReleaseCharge();
        }
    }
    private IEnumerator FireAlternating()
    {
        int currentBarrel = 0;
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if(timer >= barrelList[currentBarrel].Delay && !barrelList[currentBarrel].IsCharging)
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
        while (true)
        {
            for (int i = 0; i < barrelList.Count; i++)
            {
                timers[i] += Time.deltaTime;
                if (timers[i] >= barrelList[i].Delay && !barrelList[i].IsCharging)
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
