using UnityEngine;

public class Patroling : MonoBehaviour
{
    private Transform PointA;
    private Transform PointB;
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 1;
    [SerializeField] private float idleTime = 2f; // Time to wait at each point
    private bool foundPlayer = false;
    private bool investigating = false;
    private bool chasing = false;

    private Transform currentPoint;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isIdle = false;
    private float idleTimer = 0f;
    private Vector2 lastSeenPosition;

    [SerializeField] private float fovAngle = 20f;
    [SerializeField] private float detectionRange = 1.5f;
    [SerializeField] private float GizmosDetectionRange = 1f;
    [SerializeField] private Transform fovPoint;
    private bool playerDetected = false;
    private float detectionTimer = 0f;
    [SerializeField] private float timeToBust = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Transform parent = transform.parent;

        if (parent != null)
        {
            PointA = parent.Find("PointA");
            PointB = parent.Find("PointB");

            if (PointA != null && PointB != null)
            {
                currentPoint = PointB;
            }
            else
            {
                Debug.LogError("Patroling: PointA or PointB not found");
            }
        }
    }

    void Update()
    {
        DetectPlayer();

        if (chasing)
        {
            ChasePlayer();
        }
        else if (investigating)
        {
            Investigate();
        }
        else if (foundPlayer)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
        }
        else if (isIdle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTime)
            {
                isIdle = false;
                idleTimer = 0f;
                currentPoint = (currentPoint == PointA) ? PointB : PointA;
            }
        }
        else
        {
            PatrolBehavior();
        }
    }

    private void PatrolBehavior()
    {

        rb.linearVelocity = new Vector2( (currentPoint == PointA) ? -speed : speed, 0);
        spriteRenderer.flipX = currentPoint == PointA;
        FlipFOV(spriteRenderer.flipX);

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.25f)
        {
            isIdle = true;
            rb.linearVelocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
        }
    }

    private void ChasePlayer()
    {
        Vector2 targetPosition = player.transform.position;
        Vector2 direction = targetPosition - (Vector2)transform.position;
        rb.linearVelocity = direction.normalized * (speed * 1.5f);
        spriteRenderer.flipX = (direction.x < 0);
        FlipFOV(spriteRenderer.flipX);
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
    }

    private void Investigate()
    {
        Vector2 direction = lastSeenPosition - (Vector2)transform.position;
        if (direction.magnitude < 0.25f)
        {
            investigating = false;
            currentPoint = PointA; // Return to patrol
        }
        else
        {
            rb.linearVelocity = direction.normalized * speed;
            spriteRenderer.flipX = (direction.x < 0);
            FlipFOV(spriteRenderer.flipX);
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        }
    }

    private void DetectPlayer()
    {
        Vector2 directionToPlayer = player.transform.position - transform.position;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
            Debug.Log("Player found");
            if (playerStatus == null || !playerStatus.isInvisible)
            {
                if (!foundPlayer)
                {
                    foundPlayer = true;
                    investigating = true;
                    lastSeenPosition = player.transform.position;
                }
                else
                {
                    chasing = true;
                }
            }
        }
    }

    private void FlipFOV(bool flip)
    {
        
        if (flip)
        {
            fovPoint.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            fovPoint.rotation = Quaternion.Euler(0, 0, -90);
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