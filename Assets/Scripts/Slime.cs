using UnityEngine;

public class Slime : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 m_TargetPosition;
    private Vector2 m_Offset;

    void Start()
    {
        // Get random offset to prevent slimes from stacking
        m_Offset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
    }

    private SlimeSpawner m_Spawner;

    public void SetSpawner(SlimeSpawner spawner)
    {
        m_Spawner = spawner;
    }

    void Update()
    {
        // Get cursor position
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Apply random offset
        m_TargetPosition = mousePosition + m_Offset;

        // Clamp to screen bounds
        Vector2 screenBounds = GetScreenBounds();
        m_TargetPosition.x = Mathf.Clamp(m_TargetPosition.x, -screenBounds.x, screenBounds.x);
        m_TargetPosition.y = Mathf.Clamp(m_TargetPosition.y, -screenBounds.y, screenBounds.y);

        // Move slime
        transform.position = Vector2.MoveTowards(transform.position, m_TargetPosition, speed * Time.deltaTime);
    }

    // Get screen bounds in world units
    Vector2 GetScreenBounds()
    {
        Camera cam = Camera.main;
        Vector3 screenSize = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        return new Vector2(screenSize.x - 0.5f, screenSize.y - 0.5f); // Add padding
    }

    void OnMouseDown()
    {
        if (m_Spawner != null)
        {
            m_Spawner.OnSlimeDestroyed();
        }
        Destroy(gameObject);
    }
}
