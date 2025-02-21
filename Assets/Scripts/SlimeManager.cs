using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    private float currentHealth;
    public float CurrentHealth => currentHealth;

    [Header("Drop Settings")]
    [SerializeField] private float baseJuiceDrop = 1f;
    [SerializeField] private float baseExpDrop = 1f;
    [SerializeField] private AudioSource slimeHitSound;

    private SlimeData data;

    public void Initialize(SlimeData slimeData)
    {
        data = slimeData;
        currentHealth = data.health;
        baseExpDrop = data.expReward;
        baseJuiceDrop = data.juiceReward;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        slimeHitSound.Play();

        if (currentHealth <= 0)
        {
            DropResources();
            Destroy(gameObject);
        }
    }

    private void DropResources()
    {
        GameManager.Instance.AddSlimeJuice((int)baseJuiceDrop);
        GameManager.Instance.AddExperience(baseExpDrop);
    }
}
