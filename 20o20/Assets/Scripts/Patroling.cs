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
    private GameController gameController;
    private PlayerStatus ps;
    [SerializeField] private GameObject yellowInterrogation, redExclamation;
    LayerMask layerMask;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Transform parent = transform.parent;
        gameController = FindFirstObjectByType<GameController>();
        if (yellowInterrogation != null) yellowInterrogation.SetActive(false);
        if (redExclamation != null) redExclamation.SetActive(false);
        layerMask = ~(1 << gameObject.layer | 1 << LayerMask.NameToLayer("NotDetectedByEnemy"));

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

        if (player != null)
        {
            ps = player.GetComponent<PlayerStatus>();
        }
        else
        {
            Debug.LogError("Patroling: Player not found");
        }

    }

    void Update()
    {
        DetectPlayer();
        //Debug.Log(ps.isInvisible);

        if (playerDetected)
        {
            chasing = true;
            investigating = false;
        }

        if (chasing)
        {
            redExclamation.SetActive(true);
            yellowInterrogation.SetActive(false);
            ChasePlayer();
        }
        else if (investigating)
        {
            redExclamation.SetActive(false);
            yellowInterrogation.SetActive(true);
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

        Vector2 direction = currentPoint.position - transform.position;
        rb.linearVelocity = new Vector2( (direction.x < 0) ? -speed : speed, 0);
        spriteRenderer.flipX = direction.x < 0;
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

        if (!ps.isInvisible) {
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
        else {
            chasing = false;
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
                playerDetected = false;
                currentPoint = PointA; // Return to patrol
                detectionTimer = 0f;
                redExclamation.SetActive(false);
                yellowInterrogation.SetActive(false);
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

        RaycastHit2D hit = Physics2D.Raycast(fovPoint.position, directionToPlayer, detectionRange, layerMask);

        playerDetected = false;

        if (angle < fovAngle / 2) 
        {
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                if (!ps.isInvisible) playerDetected = true;
            }
        }
        if (directionToPlayer.magnitude < 0.5f) {
            if (!ps.isInvisible) playerDetected = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            if (ps == null || !ps.isInvisible)
            {
                if (chasing && !ps.IsOnCooldown())
                {
                    ps.DecreaseLife();
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