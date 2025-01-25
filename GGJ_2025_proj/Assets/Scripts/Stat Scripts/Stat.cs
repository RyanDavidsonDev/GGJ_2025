using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    [SerializeField] private float value;
    public float Value{get{return value;}set{value = value;}}
}
