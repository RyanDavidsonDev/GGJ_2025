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
    [SerializeField] private float ephemeralPercentModifer;
    public float Value{ get { return total; } }
    public float BaseValue { get { return baseValue; } }
    public float BaseModifier { get { return baseModifier; } set { baseModifier = value; RecalculateTotal(); } }
    public float PercentModifier { get { return percentModifier; } set { percentModifier = value; RecalculateTotal(); } }
    public float BonusModifier { get { return bonusModifier; } set { bonusModifier = value; RecalculateTotal(); } }
    public float EphemeralModifier { get { return ephemeralPercentModifer; } set { ephemeralPercentModifer = value; RecalculateTotal(); } }

    public void ChangeBaseValue(float value)
    {
        baseValue = value;
        RecalculateTotal();
    }
    public void ClearEphemeralModifier()
    {
        ephemeralPercentModifer = 0;
        RecalculateTotal();
    }
    protected virtual void RecalculateTotal()
    {
        total = (baseValue + baseModifier) * (1f + percentModifier + ephemeralPercentModifer) + bonusModifier;
    }
    private void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        RecalculateTotal();
    }
}
