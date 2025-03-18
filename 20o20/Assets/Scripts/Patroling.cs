using UnityEngine;

public class Patroling : MonoBehaviour
{
    private Transform PointA;
    private Transform PointB;
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 1;
    [SerializeField] private float idleTime = 2f; // Time to wait at each point
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

        if (playerDetected)
        {
            chasing = true;
            investigating = false;
        }

        if (chasing)
        {
            ChasePlayer();
        }
        else if (investigating)
        {
            Investigate();
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

        if (!playerDetected) {
            detectionTimer += Time.deltaTime;
            if (detectionTimer >= timeToBust)
            {
                chasing = false;
                investigating = true;
                lastSeenPosition = player.transform.position;
                detectionTimer = 0f;
            }
        }
        else
        {
            detectionTimer = 0f;
        }

    }

    private void Investigate()
    {
        Vector2 direction = lastSeenPosition - (Vector2)transform.position;
        if (direction.x < 0.25f) 
        {
            if (detectionTimer >= timeToBust)
            {
                investigating = false;
                chasing = false;
                currentPoint = PointA; // Return to patrol
                detectionTimer = 0f;
            }
            else
            {
                detectionTimer += Time.deltaTime;
                rb.linearVelocity = Vector2.zero;
                animator.SetFloat("Speed", 0);
            }

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
        Vector2 directionToPlayer = player.transform.position - fovPoint.position;
        float angle = Vector2.Angle(fovPoint.up, directionToPlayer);

        int layerMask = ~(1 << gameObject.layer);

        RaycastHit2D hit = Physics2D.Raycast(fovPoint.position, directionToPlayer, detectionRange, layerMask);

        playerDetected = false;

        //Debug.Log("Direction to player: " + directionToPlayer);
        //Debug.Log("Angle to player: " + angle);
        //Debug.Log("Raycast hit: " + (hit.collider != null ? hit.collider.name : "None"));

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
            if (playerStatus == null || !playerStatus.isInvisible)
            {
                //Debug.Log("Perdemo");
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