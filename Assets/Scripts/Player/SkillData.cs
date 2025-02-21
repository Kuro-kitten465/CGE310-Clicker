using System;
using UnityEngine;

[Serializable]
public class SkillData
{
    public string skillName;
    public int level = 0;
    public float baseValue;
    public float valuePerLevel;
    public int baseCost;
    public float costMultiplier = 1.5f; // Cost increases by 50% per level
    
    public float CurrentValue => baseValue + (valuePerLevel * level);
    public int CurrentCost => Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level));
}
