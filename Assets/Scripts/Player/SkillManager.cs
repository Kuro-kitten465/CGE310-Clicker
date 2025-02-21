using UnityEngine;
using UnityEngine.Events;

public class SkillManager : MonoBehaviour
{
    [Header("Skill Configurations")]
    [SerializeField] private SkillData baseAttack = new SkillData
    {
        skillName = "Base Attack",
        baseValue = 1f,
        valuePerLevel = 1f,
        baseCost = 10
    };
    
    [SerializeField] private SkillData critDamage = new SkillData
    {
        skillName = "Critical Damage",
        baseValue = 150f, // 150% base crit damage
        valuePerLevel = 10f, // +10% per level
        baseCost = 15
    };
    
    [SerializeField] private SkillData critRate = new SkillData
    {
        skillName = "Critical Rate",
        baseValue = 5f, // 5% base crit rate
        valuePerLevel = 2f, // +2% per level
        baseCost = 15
    };
    
    [SerializeField] private SkillData juiceBonus = new SkillData
    {
        skillName = "Juice Bonus",
        baseValue = 0f,
        valuePerLevel = 10f, // +10% per level
        baseCost = 20
    };
    
    [SerializeField] private SkillData expBonus = new SkillData
    {
        skillName = "EXP Bonus",
        baseValue = 0f,
        valuePerLevel = 10f, // +10% per level
        baseCost = 20
    };

    [Header("Auto Kill Settings")]
    [SerializeField] private bool isAutoKillUnlocked = false;
    [SerializeField] private int autoKillUnlockCost = 10000;
    [SerializeField] private float autoKillInterval = 1f;
    private float autoKillTimer;

    private void Update()
    {
        if (isAutoKillUnlocked)
        {
            HandleAutoKill();
        }
    }

    private void HandleAutoKill()
    {
        autoKillTimer += Time.deltaTime;
        if (autoKillTimer >= autoKillInterval)
        {
            autoKillTimer = 0f;
            AutoKillNearestSlime();
        }
    }

    private void AutoKillNearestSlime()
    {
        GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
        if (slimes.Length > 0)
        {
            // Find nearest slime
            GameObject nearestSlime = null;
            float nearestDistance = float.MaxValue;
            Vector3 playerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            foreach (GameObject slime in slimes)
            {
                float distance = Vector2.Distance(playerPos, slime.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestSlime = slime;
                }
            }

            // Deal damage to nearest slime
            if (nearestSlime != null)
            {
                SlimeManager slime = nearestSlime.GetComponent<SlimeManager>();
                if (slime != null)
                {
                    slime.TakeDamage(slime.CurrentHealth);
                }
            }
        }
    }

    public float CalculateDamage()
    {
        float damage = baseAttack.CurrentValue;
        
        // Check for critical hit
        if (UnityEngine.Random.Range(0f, 100f) < critRate.CurrentValue)
        {
            damage *= (critDamage.CurrentValue / 100f);
        }
        
        return damage;
    }

    public float GetJuiceMultiplier()
    {
        return 1f + (juiceBonus.CurrentValue / 100f);
    }

    public float GetExpMultiplier()
    {
        return 1f + (expBonus.CurrentValue / 100f);
    }

    public void TryUpgradeSkill(SkillData skill)
    {
        int cost = skill.CurrentCost;
        if (GameManager.Instance.playerStats.slimeJuice >= cost)
        {
            GameManager.Instance.playerStats.slimeJuice -= cost;
            skill.level++;
        }
    }

    public void TryUnlockAutoKill(GameObject obj1, GameObject obj2)
    {
        if (!isAutoKillUnlocked && GameManager.Instance.playerStats.slimeJuice >= autoKillUnlockCost)
        {
            GameManager.Instance.playerStats.slimeJuice -= autoKillUnlockCost;
            isAutoKillUnlocked = true;
            obj1.SetActive(false);
            obj2.SetActive(false);
        }
    }

    // Getter methods for UI
    public SkillData GetBaseAttack() => baseAttack;
    public SkillData GetCritDamage() => critDamage;
    public SkillData GetCritRate() => critRate;
    public SkillData GetJuiceBonus() => juiceBonus;
    public SkillData GetExpBonus() => expBonus;
    public bool IsAutoKillUnlocked() => isAutoKillUnlocked;
    public void UnlockAutoKill(bool value) => isAutoKillUnlocked = value;
    public int GetAutoKillCost() => autoKillUnlockCost;
    public void LoadSkillData(SkillData[] skillDatas)
    {
        foreach (var skillData in skillDatas)
        {
            switch (skillData.skillName)
            {
                case "BaseAttack":
                    baseAttack = skillData;
                    break;
                case "CriticalDamage":
                    critDamage = skillData;
                    break;
                case "CriticalRate":
                    critRate = skillData;
                    break;
                case "JuiceBonus":
                    juiceBonus = skillData;
                    break;
                case "EXPBonus":
                    expBonus = skillData;
                    break;
            }
        }
    }
}
