using UnityEngine;
using KuroNeko.Utilities.DesignPattern;
using UnityEngine.Rendering;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("References")]
    [SerializeField] private LayerMask slimeLayer;
    [SerializeField] private float clickRadius = 1f;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource levelSound;

    [Header("Boundary Settings")]
    [SerializeField] private float boundaryLeft = -8f;
    [SerializeField] private float boundaryRight = 8f;
    [SerializeField] private float boundaryTop = 4f;
    [SerializeField] private float boundaryBottom = -4f;

    public static float BoundaryLeft => Instance.boundaryLeft;
    public static float BoundaryRight => Instance.boundaryRight;
    public static float BoundaryTop => Instance.boundaryTop;
    public static float BoundaryBottom => Instance.boundaryBottom;
    
    public PlayerStats playerStats = new PlayerStats();

    private Camera mainCamera;
    private Vector3 vector3;

    private void Start()
    {
        mainCamera = Camera.main;
        attackPoint.gameObject.transform.localScale = new Vector3(clickRadius * 10f, clickRadius * 10f, 1);
    }

    private void Update()
    {
        MousePos(out Vector3 worldPosition);

        if (Input.GetMouseButtonDown(0))
        {
            clickSound.Play();
            HandleClick(worldPosition);
        }

        #if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        #endif
    }

    private void MousePos(out Vector3 vector)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePos);
        attackPoint.position = worldPosition;
        vector = worldPosition;
    }

    private void HandleClick(Vector3 worldPosition)
    {
        // Get all slimes in click radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(worldPosition, clickRadius, slimeLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            var slimeHealth = hitCollider.GetComponent<SlimeManager>();
            if (slimeHealth != null)
            {
                var damage = skillManager.CalculateDamage();
                slimeHealth.TakeDamage(damage);
            }
        }
    }

    public void AddSlimeJuice(int amount)
    {
        playerStats.slimeJuice += Mathf.RoundToInt(amount * skillManager.GetJuiceMultiplier());
    }

    public void AddExperience(float amount)
    {
        playerStats.experience += amount * skillManager.GetExpMultiplier();
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        float experienceNeeded = playerStats.level * 10f; // Simple level-up formula
        if (playerStats.experience >= experienceNeeded)
        {
            playerStats.level++;
            levelSound.Play();
            playerStats.experience -= experienceNeeded;
            // You can add level-up rewards here
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(vector3, clickRadius);
    }
    #endif
}
