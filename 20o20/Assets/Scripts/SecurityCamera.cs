using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] private float fovAngle = 45f;
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private Transform player;
    [SerializeField] private Transform fovPoint;
    [SerializeField] private float timeToBust = 3f;
    [SerializeField] private float flipInterval = 2f;
    private float detectionTimer = 0f;
    private bool playerDetected = false;
    private float flipTimer = 0f;
    private bool facingRight = true;

    void Start()
    {
        // Initialize facing direction based on current scale
        facingRight = transform.localScale.x > 0;
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
                Debug.Log("Player busted");
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
                Debug.Log("Player detected");
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (fovPoint == null) return;
//
    //    Gizmos.color = Color.red;
    //    Vector3 leftBoundary = Quaternion.Euler(0, 0, fovAngle / 2) * fovPoint.up * detectionRange;
    //    Vector3 rightBoundary = Quaternion.Euler(0, 0, -fovAngle / 2) * fovPoint.up * detectionRange;
//
    //    Gizmos.DrawLine(fovPoint.position, fovPoint.position + leftBoundary);
    //    Gizmos.DrawLine(fovPoint.position, fovPoint.position + rightBoundary);
    //    Gizmos.DrawWireSphere(fovPoint.position, detectionRange);
    //}
}