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

    private void Update()
    {
        juiceText.text = GameManager.Instance.playerStats.slimeJuice.ToString();
        UpdateExperienceBar();
    }

    public void UpdateExperienceBar()
    {
        levelText.text = GameManager.Instance.playerStats.level.ToString();
        experienceBar.value = GameManager.Instance.playerStats.experience;
        experienceBar.maxValue = GameManager.Instance.playerStats.level * 10f;
    }
}
