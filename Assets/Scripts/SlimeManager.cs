using UnityEngine;
using UnityEngine.Events;

public class SlimeManager : MonoBehaviour
{
    [Header("Slime Settings")]
    [SerializeField] private float maxHealth = 1f;
    private float currentHealth;

    [Header("Drop Settings")]
    [SerializeField] private float baseJuiceDrop = 1f;
    [SerializeField] private float baseExpDrop = 1f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
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
