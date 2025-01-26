using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP_Orb : MonoBehaviour
{
    [SerializeField] private Collider orbCollider;
    [SerializeField] private int xpAmount;

    private void Start()
    {
        orbCollider = GetComponent<Collider>();
    }

    public int GetXP()
    {
        return xpAmount;
    }
}
