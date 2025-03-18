using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] private float fovAngle = 45f;
    [SerializeField] private float detectionRange = 1.2f;
    [SerializeField] private Transform player;
    [SerializeField] private Transform fovPoint;
    [SerializeField] private float timeToBust = 3f;
    [SerializeField] private float flipInterval = 2f;
    [SerializeField] private float GizmosDetectionRange = 1f;
    private float detectionTimer = 0f;
    private bool playerDetected = false;
    private float flipTimer = 0f;
    private bool facingRight = true;
    private GameController gameController;

    void Start()
    {
        // Initialize facing direction based on current scale
        facingRight = transform.localScale.x > 0;
        gameController = FindFirstObjectByType<GameController>();
        
    }

    void Update()
    {
        DetectPlayer();
        RotateCamera();

        if(playerDetected)
        {
            detectionTimer += Time.deltaTime;
            if(detectionTimer >= timeToBust)
            {
                if(gameController != null)
                {
                    gameController.GameOver();
                }
            }
        } else
        {
            detectionTimer = 0;
        }
    }

    private void RotateCamera()
    {
        flipTimer += Time.deltaTime;

        if (flipTimer >= flipInterval)
        {
            flipTimer = 0f;
            FlipCamera();
        }
    }

    private void FlipCamera()
    {
        facingRight = !facingRight;
        
        Vector3 scale = transform.localScale;
        
        scale.x *= -1;
        
        transform.localScale = scale;
    }

    private void DetectPlayer()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, fovPoint.up);
        RaycastHit2D hit = Physics2D.Raycast(fovPoint.position, directionToPlayer, detectionRange);

        playerDetected = false;

        if (angle < fovAngle / 2)
        {
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                playerDetected = true;
                //Debug.Log("Player detected");
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (fovPoint == null) return;

        Gizmos.color = Color.green;
        if (playerDetected)
        {
            Gizmos.color = new Color(1.0f, 0.5f, 0.0f);
            if (detectionTimer >= timeToBust)
            {
                Gizmos.color = Color.red;
            }
        }
        int segments = 100;
        float angleStep = fovAngle / segments;

        // Draw filled area
        Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.2f);
        for (int i = 0; i < segments; i++)
        {
            float currentAngle = -fovAngle / 2 + i * angleStep;
            float nextAngle = -fovAngle / 2 + (i + 1) * angleStep;
            Vector3 currentPoint = fovPoint.position + Quaternion.Euler(0, 0, currentAngle) * fovPoint.up * GizmosDetectionRange;
            Vector3 nextPoint = fovPoint.position + Quaternion.Euler(0, 0, nextAngle) * fovPoint.up * GizmosDetectionRange;
            Gizmos.DrawLine(fovPoint.position, currentPoint);
            Gizmos.DrawLine(currentPoint, nextPoint);
            Gizmos.DrawLine(nextPoint, fovPoint.position);
        }
    }
}