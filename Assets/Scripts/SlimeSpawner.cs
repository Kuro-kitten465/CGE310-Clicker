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

    private float spawnTimer;

    private void Update()
    {
        if (!isHugeMode)
        {
            HandleNormalSpawning();
        }
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

    private void SpawnSlimeBasedOnLevel()
    {
        int level = GameManager.Instance.playerStats.level;
        GameObject prefabToSpawn;

        if (level >= 30)
        {
            StartHugeSlimeMode();
            return;
        }
        else if (level >= 20)
        {
            prefabToSpawn = largeSlimePrefab;
        }
        else if (level >= 10)
        {
            prefabToSpawn = mediumSlimePrefab;
        }
        else
        {
            prefabToSpawn = smallSlimePrefab;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
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
        Instantiate(hugeSlimePrefab, spawnPosition, Quaternion.identity);

        // Wait until huge slime is defeated
        while (GameObject.FindGameObjectsWithTag("Slime").Length > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // Award level up and restart
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
