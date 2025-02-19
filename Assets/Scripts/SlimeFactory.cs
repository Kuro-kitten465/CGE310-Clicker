using UnityEngine;

public class SlimeFactory : MonoBehaviour
{
    public GameObject slimePrefab; // Assign in Inspector

    public GameObject CreateSlime(Vector2 position)
    {
        return Instantiate(slimePrefab, position, Quaternion.identity);
    }
}
