using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringController : MonoBehaviour
{
    [SerializeField] private FireMode fireMode;
    [SerializeField] private List<Barrel> barrelList;
}
public enum FireMode
{
    Alternating,
    Simultanious
}
