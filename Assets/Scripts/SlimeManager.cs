using UnityEngine;
using UnityEngine.Events;

public class SlimeManager : MonoBehaviour
{
    private float currentHealth;

    [Header("Drop Settings")]
    [SerializeField] private float baseJuiceDrop = 1f;
    [SerializeField] private float baseExpDrop = 1f;

    private SlimeData data;

    public void Initialize(SlimeData slimeData)
    {
        data = slimeData;
        currentHealth = data.health;
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
