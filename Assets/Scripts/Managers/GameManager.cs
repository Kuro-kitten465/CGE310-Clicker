using UnityEngine;
using KuroNeko.Utilities.DesignPattern;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("References")]
    [SerializeField] private LayerMask slimeLayer;
    [SerializeField] private float clickRadius = 1f;
    
    public PlayerStats playerStats = new PlayerStats();

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePos);

        // Get all slimes in click radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(worldPosition, clickRadius, slimeLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            var slimeHealth = hitCollider.GetComponent<SlimeManager>();
            if (slimeHealth != null)
            {
                slimeHealth.TakeDamage(playerStats.clickDamage);
            }
        }
    }

    public void AddSlimeJuice(int amount)
    {
        playerStats.slimeJuice += amount;
    }

    public void AddExperience(float amount)
    {
        playerStats.experience += amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        float experienceNeeded = playerStats.level * 10f; // Simple level-up formula
        if (playerStats.experience >= experienceNeeded)
        {
            playerStats.level++;
            playerStats.experience -= experienceNeeded;
            // You can add level-up rewards here
        }
    }
}
