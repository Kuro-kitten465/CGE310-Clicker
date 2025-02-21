using System.Collections;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    [Header("Slime Prefabs")]
    [SerializeField] private GameObject smallSlimePrefab;
    [SerializeField] private GameObject mediumSlimePrefab;
    [SerializeField] private GameObject largeSlimePrefab;
    [SerializeField] private GameObject hugeSlimePrefab;
    
    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private int maxNormalSlimes = 20;
    [SerializeField] private bool isHugeMode = false;

    [Header("Base Stats")]
    [SerializeField] private float smallSlimeBaseHP = 1f;
    [SerializeField] private float mediumSlimeBaseHP = 3f;
    [SerializeField] private float largeSlimeBaseHP = 5f;
    [SerializeField] private float hugeSlimeBaseHP = 20f;
    [SerializeField] private float smallSlimeBaseExp = 1f;
    [SerializeField] private float mediumSlimeBaseExp = 3f;
    [SerializeField] private float largeSlimeBaseExp = 5f;
    [SerializeField] private float hugeSlimeBaseExp = 0f;
    [SerializeField] private int smallSlimeBaseJuice = 1;
    [SerializeField] private int mediumSlimeBaseJuice = 2;
    [SerializeField] private int largeSlimeBaseJuice = 3;
    [SerializeField] private int hugeSlimeBaseJuice = 5;

    private int hugeSlimesDefeated = 0;
    private float statMultiplier = 1f;
    private float spawnTimer;

    public Color[] colors = new Color[]
    {
        SlimeColor.GreenColor,
        SlimeColor.BlueColor,
        SlimeColor.RedColor,
        SlimeColor.YellowColor,
        SlimeColor.PurpleColor,
        SlimeColor.OrangeColor
    };

    private void Start()
    {
        UpdateStatMultiplier();
    }

    private void Update()
    {
        if (!isHugeMode)
        {
            HandleNormalSpawning();
        }
    }

    private void UpdateStatMultiplier()
    {
        // Each huge slime defeated increases multiplier by 10
        statMultiplier = Mathf.Pow(10, hugeSlimesDefeated);
    }

    private void HandleNormalSpawning()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnSlimeIfNeeded();
        }
    }

    private void SpawnSlimeIfNeeded()
    {
        int currentSlimeCount = GameObject.FindGameObjectsWithTag("Slime").Length;
        if (currentSlimeCount < maxNormalSlimes)
        {
            SpawnSlimeBasedOnLevel();
        }
    }

    private void ConfigureSlimeStats(GameObject slime, float baseHP, float baseExp, int baseJuice)
    {
        SlimeManager slimeManager = slime.GetComponent<SlimeManager>();
        if (slimeManager != null)
        {
            float scaledHP = baseHP * statMultiplier;
            float scaledExp = baseExp * statMultiplier;
            float scaledJuice = baseExp * statMultiplier;
            slimeManager.Initialize(new SlimeData() {
                health = scaledHP,
                expReward = scaledExp,
                juiceReward = Mathf.RoundToInt(scaledJuice)
            });
        }
    }

    private void SpawnSlimeBasedOnLevel()
    {
        int level = GameManager.Instance.playerStats.level % 31;
        GameObject prefabToSpawn;
        float baseHP;
        float baseExp;
        int baseJuice;

        if (level >= 30)
        {
            StartHugeSlimeMode();
            return;
        }
        else if (level >= 20)
        {
            prefabToSpawn = largeSlimePrefab;
            baseHP = largeSlimeBaseHP;
            baseExp = largeSlimeBaseExp;
            baseJuice = largeSlimeBaseJuice;
        }
        else if (level >= 10)
        {
            prefabToSpawn = mediumSlimePrefab;
            baseHP = mediumSlimeBaseHP;
            baseExp = mediumSlimeBaseExp;
            baseJuice = mediumSlimeBaseJuice;
        }
        else
        {
            prefabToSpawn = smallSlimePrefab;
            baseHP = smallSlimeBaseHP;
            baseExp = smallSlimeBaseExp;
            baseJuice = smallSlimeBaseJuice;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newSlime = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        var slimeRenderer = newSlime.GetComponent<SpriteRenderer>();
        if (slimeRenderer != null)
        {
            slimeRenderer.color = colors[Random.Range(0, colors.Length)];
        }
        ConfigureSlimeStats(newSlime, baseHP, baseExp, baseJuice);
    }

    private IEnumerator HugeSlimeMode()
    {
        isHugeMode = true;

        // Clear existing slimes
        GameObject[] existingSlimes = GameObject.FindGameObjectsWithTag("Slime");
        foreach (GameObject slime in existingSlimes)
        {
            Destroy(slime);
        }

        yield return new WaitForSeconds(2f); // Dramatic pause

        // Spawn huge slime
        Vector3 spawnPosition = new Vector3(0, 0, 0); // Spawn in center
        GameObject hugeSlime = Instantiate(hugeSlimePrefab, spawnPosition, Quaternion.identity);
        var slimeRenderer = hugeSlime.GetComponent<SpriteRenderer>();
        if (slimeRenderer != null)
        {
            slimeRenderer.color = colors[Random.Range(0, colors.Length)];
        }
        ConfigureSlimeStats(hugeSlime, hugeSlimeBaseHP, hugeSlimeBaseExp, hugeSlimeBaseJuice);

        // Wait until huge slime is defeated
        while (GameObject.FindGameObjectsWithTag("Slime").Length > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // Award level up and restart
        hugeSlimesDefeated++;
        UpdateStatMultiplier();
        GameManager.Instance.playerStats.level++;
        isHugeMode = false;
    }

    public void StartHugeSlimeMode()
    {
        if (!isHugeMode)
        {
            StartCoroutine(HugeSlimeMode());
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(
            Random.Range(-8f, 8f),
            Random.Range(-4f, 4f),
            0f
        );
    }
}
