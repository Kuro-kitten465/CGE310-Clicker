using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    [Header("Juice Count")]
    [SerializeField] private TMP_Text juiceText;

    [Header("Experience Bar")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Slider experienceBar;

    [Header("Damage Upgrade")]
    //[SerializeField] private GameObject damageUpgradePanel;
    [SerializeField] private TMP_Text damageUpgradeCostText;
    [SerializeField] private TMP_Text damageUpgradeLevelText;
    [SerializeField] private TMP_Text damageUpgradeDescriptText;

    [Header("CritDMG Upgrade")]
    //[SerializeField] private GameObject critDMGUpgradePanel;
    [SerializeField] private TMP_Text critDMGUpgradeCostText;
    [SerializeField] private TMP_Text critDMGUpgradeLevelText;
    [SerializeField] private TMP_Text critDMGUpgradeDescriptText;

    [Header("Crit Rate Upgrade")]
    //[SerializeField] private GameObject critRateUpgradePanel;
    [SerializeField] private TMP_Text critRateUpgradeCostText;
    [SerializeField] private TMP_Text critRateUpgradeLevelText;
    [SerializeField] private TMP_Text criteRateUpgradeDescriptText;

    [Header("Juice Bonus Upgrade")]
    //[SerializeField] private GameObject juiceUpgradePanel;
    [SerializeField] private TMP_Text juiceUpgradeCostText;
    [SerializeField] private TMP_Text juiceUpgradeLevelText;
    [SerializeField] private TMP_Text juiceUpgradeDescriptText;

    [Header("EXP Bonus Upgrade")]
    //[SerializeField] private GameObject expUpgradePanel;
    [SerializeField] private TMP_Text expUpgradeCostText;
    [SerializeField] private TMP_Text expUpgradeLevelText;
    [SerializeField] private TMP_Text expUpgradeDescriptText;

    [Header("Auto Kill")]
    [SerializeField] private GameObject autoKillBTN;
    [SerializeField] private TMP_Text autoKillCostText;

    [Header("Save")]
    [SerializeField] private GameObject saveTxtPopup;

    [Header("Config")]
    [SerializeField] private SkillManager skillManager;

    private void Update()
    {
        juiceText.text = GameManager.Instance.playerStats.slimeJuice.ToString();
        UpdateExperienceBar();
        UpdateDamageUpgradePanel();
        UpdateCritDMGUpgradePanel();
        UpdateCritRateUpgradePanel();
        UpdateJuiceUpgradePanel();
        UpdateExpUpgradePanel();
        UpdateAutoKillPanel();

        if (!skillManager.IsAutoKillUnlocked())
        {
            autoKillBTN.SetActive(true);
            autoKillCostText.gameObject.SetActive(true);
        }
        else
        {
            autoKillBTN.SetActive(false);
            autoKillCostText.gameObject.SetActive(false);
        }
    }

    #region UI Updates
    private void UpdateExperienceBar()
    {
        levelText.text = "Lvl" + GameManager.Instance.playerStats.level.ToString();
        experienceBar.value = GameManager.Instance.playerStats.experience;
        experienceBar.maxValue = GameManager.Instance.playerStats.level * 10f;
    }

    public void ShowSavePopup()
    {
        StopCoroutine(HideSavePopup());
        StartCoroutine(HideSavePopup());
    }

    IEnumerator HideSavePopup()
    {
        saveTxtPopup.SetActive(true);
        yield return new WaitForSeconds(2f);
        saveTxtPopup.SetActive(false);
    }

    private void UpdateDamageUpgradePanel()
    {
        damageUpgradeCostText.text = skillManager.GetBaseAttack().CurrentCost.ToString();
        damageUpgradeLevelText.text = "Level " + skillManager.GetBaseAttack().level.ToString();
        damageUpgradeDescriptText.text = $"Increase base damage by {skillManager.GetBaseAttack().CurrentValue} (+{skillManager.GetBaseAttack().valuePerLevel} per level)";
    }

    private void UpdateCritDMGUpgradePanel()
    {
        critDMGUpgradeCostText.text = skillManager.GetCritDamage().CurrentCost.ToString();
        critDMGUpgradeLevelText.text = "Level " + skillManager.GetCritDamage().level.ToString();
        critDMGUpgradeDescriptText.text = $"Increase crit damage by {skillManager.GetCritDamage().CurrentValue}% (+{skillManager.GetCritDamage().valuePerLevel}% per level)";
    }

    private void UpdateCritRateUpgradePanel()
    {
        critRateUpgradeCostText.text = skillManager.GetCritRate().CurrentCost.ToString();
        critRateUpgradeLevelText.text = "Level " + skillManager.GetCritRate().level.ToString();
        criteRateUpgradeDescriptText.text = $"Increase crit rate by {skillManager.GetCritRate().CurrentValue}% (+{skillManager.GetCritRate().valuePerLevel}% per level)";
    }

    private void UpdateJuiceUpgradePanel()
    {
        juiceUpgradeCostText.text = skillManager.GetJuiceBonus().CurrentCost.ToString();
        juiceUpgradeLevelText.text = "Level " + skillManager.GetJuiceBonus().level.ToString();
        juiceUpgradeDescriptText.text = $"Increase juice bonus by {skillManager.GetJuiceBonus().CurrentValue}% (+{skillManager.GetJuiceBonus().valuePerLevel}% per level)";
    }

    private void UpdateExpUpgradePanel()
    {
        expUpgradeCostText.text = skillManager.GetExpBonus().CurrentCost.ToString();
        expUpgradeLevelText.text = "Level " + skillManager.GetExpBonus().level.ToString();
        expUpgradeDescriptText.text = $"Increase exp bonus by {skillManager.GetExpBonus().CurrentValue}% (+{skillManager.GetExpBonus().valuePerLevel}% per level)";
    }

    private void UpdateAutoKillPanel()
    {
        autoKillCostText.text = skillManager.GetAutoKillCost().ToString();
    }
    #endregion

    #region Button Clicks
    public void UpgradeDamage()
    {
        skillManager.TryUpgradeSkill(skillManager.GetBaseAttack());
    }

    public void UpgradeCritDMG()
    {
        skillManager.TryUpgradeSkill(skillManager.GetCritDamage());
    }

    public void UpgradeCritRate()
    {
        skillManager.TryUpgradeSkill(skillManager.GetCritRate());
    }

    public void UpgradeJuiceBonus()
    {
        skillManager.TryUpgradeSkill(skillManager.GetJuiceBonus());
    }

    public void UpgradeExpBonus()
    {
        skillManager.TryUpgradeSkill(skillManager.GetExpBonus());
    }

    public void UnlockAutoKill()
    {
        skillManager.TryUnlockAutoKill(autoKillBTN, autoKillCostText.gameObject);
    }
    #endregion
}
