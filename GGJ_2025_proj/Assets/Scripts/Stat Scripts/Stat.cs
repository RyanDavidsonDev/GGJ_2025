using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    [SerializeField] private float baseValue;
    [SerializeField] private float baseModifier;
    [SerializeField] private float percentModifier;
    [SerializeField] private float bonusModifier;
    [SerializeField] private float total;
    public float Value{ get { return total; } }

    public float BaseModifier { get { return baseModifier; } set { baseModifier = value; RecalculateTotal(); } }
    public float PercentModifier { get { return percentModifier; } set { percentModifier = value; RecalculateTotal(); } }
    public float BonusModifier { get { return bonusModifier; } set { bonusModifier = value; RecalculateTotal(); } }

    private void RecalculateTotal()
    {
        total = (baseValue + baseModifier) * (1f + percentModifier) + bonusModifier;
    }
}
