using UnityEngine;

public class SlimeMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minDistance = 0.1f;
    
    private float boundaryLeft;
    private float boundaryRight;
    private float boundaryTop;
    private float boundaryBottom;
    
    [Header("Collision Avoidance")]
    [SerializeField] private float avoidanceRadius = 1f;
    [SerializeField] private LayerMask slimeLayer;
    
    private Camera mainCamera;
    private Vector3 targetPosition;

    private void Awake()
    {
        boundaryLeft = GameManager.BoundaryLeft;
        boundaryRight = GameManager.BoundaryRight;
        boundaryTop = GameManager.BoundaryTop;
        boundaryBottom = GameManager.BoundaryBottom;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        targetPosition = transform.position;
    }
    
    private void Update()
    {
        // Get mouse position in world coordinates
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePos);
        worldPosition.z = 0f;
        
        // Clamp target position within boundaries
        worldPosition.x = Mathf.Clamp(worldPosition.x, boundaryLeft, boundaryRight);
        worldPosition.y = Mathf.Clamp(worldPosition.y, boundaryBottom, boundaryTop);
        
        // Update target position
        targetPosition = worldPosition;
        
        // Calculate movement direction
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        
        // Apply collision avoidance
        Vector3 avoidanceForce = CalculateAvoidanceForce();
        moveDirection += avoidanceForce;
        moveDirection.Normalize();
        
        // Move towards target position while avoiding others
        if (Vector3.Distance(transform.position, targetPosition) > minDistance)
        {
            Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
            
            // Clamp the new position within boundaries
            newPosition.x = Mathf.Clamp(newPosition.x, boundaryLeft, boundaryRight);
            newPosition.y = Mathf.Clamp(newPosition.y, boundaryBottom, boundaryTop);
            
            transform.position = newPosition;
        }
    }
    
    private Vector3 CalculateAvoidanceForce()
    {
        Vector3 avoidanceForce = Vector3.zero;
        Collider2D[] nearbySlimes = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius, slimeLayer);
        
        foreach (Collider2D other in nearbySlimes)
        {
            if (other.gameObject != gameObject)
            {
                Vector3 awayFromSlime = transform.position - other.transform.position;
                float distance = awayFromSlime.magnitude;
                
                // The closer the slime, the stronger the avoidance force
                float forceMagnitude = 1 - (distance / avoidanceRadius);
                avoidanceForce += awayFromSlime.normalized * forceMagnitude;
            }
        }
        
        return avoidanceForce;
    }
    
    #if UNITY_EDITOR
    // Optional: Visualize the boundaries and avoidance radius in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(
            new Vector3((boundaryLeft + boundaryRight) / 2, (boundaryTop + boundaryBottom) / 2, 0),
            new Vector3(boundaryRight - boundaryLeft, boundaryTop - boundaryBottom, 0)
        );
        
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
        }
    }
    #endif
}
