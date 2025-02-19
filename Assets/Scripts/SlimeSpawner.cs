using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public SlimeFactory slimeFactory;  // Reference to the factory
    public float spawnRate = 2f;
    public int maxSlimes = 10;
    private int m_CurrentSlimes = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnSlime), 1f, spawnRate);
    }

    void SpawnSlime()
    {
        if (m_CurrentSlimes >= maxSlimes) return;

        Vector2 spawnPosition = GetValidSpawnPosition();
        GameObject slime = slimeFactory.CreateSlime(spawnPosition); // Use factory
        slime.GetComponent<Slime>().SetSpawner(this); // Link to spawner
        m_CurrentSlimes++;
    }

    Vector2 GetValidSpawnPosition()
    {
        Camera cam = Camera.main;
        float x = Random.Range(0.1f, 0.9f);
        float y = Random.Range(0.1f, 0.9f);
        return cam.ViewportToWorldPoint(new Vector2(x, y));
    }

    public void OnSlimeDestroyed()
    {
        m_CurrentSlimes--;
    }
}
