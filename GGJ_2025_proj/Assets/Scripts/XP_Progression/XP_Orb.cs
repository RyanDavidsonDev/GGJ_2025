using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP_Orb : MonoBehaviour
{
    [SerializeField] private Collider orbCollider;
    [SerializeField] private float xpAmount;

    private void Start()
    {
        orbCollider = GetComponent<Collider>();
    }
}
