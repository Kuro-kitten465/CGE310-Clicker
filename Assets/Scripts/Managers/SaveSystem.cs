using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private SkillManager skillManager;

    private SkillData[] skillDatas = new SkillData[5];
    private bool m_isAutoKillUnlocked;

    private void Start()
    {
        GetSkills();
        LoadGame();
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("SlimeJuice", GameManager.Instance.playerStats.slimeJuice);
        PlayerPrefs.SetFloat("Experience", GameManager.Instance.playerStats.experience);
        PlayerPrefs.SetInt("Level", GameManager.Instance.playerStats.level);

        foreach (var skillData in skillDatas)
        {
            PlayerPrefs.SetString(skillData.skillName.Trim(), skillData.skillName);
            PlayerPrefs.SetInt(skillData.skillName.Trim() + "Level", skillData.level);
            PlayerPrefs.SetFloat(skillData.skillName.Trim() + "BaseValue", skillData.baseValue);
            PlayerPrefs.SetFloat(skillData.skillName.Trim() + "ValuePerLevel", skillData.valuePerLevel);
            PlayerPrefs.SetInt(skillData.skillName.Trim() + "BaseCost", skillData.baseCost);
            PlayerPrefs.SetFloat(skillData.skillName.Trim() + "CostMultiplier", skillData.costMultiplier);
        }

        PlayerPrefs.SetInt("IsAutoKillUnlocked", m_isAutoKillUnlocked ? 1 : 0);
    }

    public void LoadGame()
    {
        GameManager.Instance.playerStats.slimeJuice = PlayerPrefs.GetInt("SlimeJuice", 0);
        GameManager.Instance.playerStats.experience = PlayerPrefs.GetFloat("Experience", 0f);
        GameManager.Instance.playerStats.level = PlayerPrefs.GetInt("Level", 1);

        for (int i = 0; i < skillDatas.Length; i++)
        {
            skillDatas[i].skillName = PlayerPrefs.GetString(skillDatas[i].skillName.Trim(), skillDatas[i].skillName);
            skillDatas[i].level = PlayerPrefs.GetInt(skillDatas[i].skillName.Trim() + "Level", skillDatas[i].level);
            skillDatas[i].baseValue = PlayerPrefs.GetFloat(skillDatas[i].skillName.Trim() + "BaseValue", skillDatas[i].baseValue);
            skillDatas[i].valuePerLevel = PlayerPrefs.GetFloat(skillDatas[i].skillName.Trim() + "ValuePerLevel", skillDatas[i].valuePerLevel);
            skillDatas[i].baseCost = PlayerPrefs.GetInt(skillDatas[i].skillName.Trim() + "BaseCost", skillDatas[i].baseCost);
            skillDatas[i].costMultiplier = PlayerPrefs.GetFloat(skillDatas[i].skillName.Trim() + "CostMultiplier", skillDatas[i].costMultiplier);
        }

        m_isAutoKillUnlocked = (PlayerPrefs.GetInt("IsAutoKillUnlocked") != 0);
        skillManager.UnlockAutoKill(m_isAutoKillUnlocked);
        skillManager.LoadSkillData(skillDatas);
    }

    public void GetSkills()
    {
        var baseAttack = skillManager.GetBaseAttack();
        var critDamage = skillManager.GetCritDamage();
        var critRate = skillManager.GetCritRate();
        var juiceBonus = skillManager.GetJuiceBonus();
        var expBonus = skillManager.GetExpBonus();
        m_isAutoKillUnlocked = skillManager.IsAutoKillUnlocked();

        skillDatas[0] = baseAttack;
        skillDatas[1] = critDamage;
        skillDatas[2] = critRate;
        skillDatas[3] = juiceBonus;
        skillDatas[4] = expBonus;
    }
}
