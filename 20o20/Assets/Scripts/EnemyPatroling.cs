using UnityEngine;

public class EnemyPatroling : MonoBehaviour
{
    private Transform PointA;
    private Transform PointB;
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 1;
    [SerializeField] private float idleTime = 2f;
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

    [SerializeField] private Transform[] waypoints; // Waypoints for multi-floor navigation
    [SerializeField] private Transform[] floorTransitions; // Stairs/Elevators

    private int currentFloor = 0; // Track current floor index

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
        Vector2 direction = currentPoint.position - transform.position;
        rb.linearVelocity = new Vector2((currentPoint == PointA ? -1 : 1) * speed, 0);
        spriteRenderer.flipX = (currentPoint == PointA);
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
        int playerFloor = GetPlayerFloor();

        if (playerFloor != currentFloor)
        {
            MoveToFloor(playerFloor);
        }
        else
        {
            Vector2 direction = targetPosition - (Vector2)transform.position;
            rb.linearVelocity = direction.normalized * (speed * 1.5f);
            spriteRenderer.flipX = (direction.x < 0);
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        }
    }

    private void Investigate()
    {
        int targetFloor = GetFloorFromPosition(lastSeenPosition);

        if (targetFloor != currentFloor)
        {
            MoveToFloor(targetFloor);
        }
        else
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
                animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
            }
        }
    }

    private void MoveToFloor(int targetFloor)
    {
        Transform closestTransition = floorTransitions[currentFloor];
        Vector2 direction = closestTransition.position - transform.position;
        rb.linearVelocity = direction.normalized * speed;
        
        if (Vector2.Distance(transform.position, closestTransition.position) < 0.25f)
        {
            currentFloor = targetFloor;
        }
    }

    private int GetPlayerFloor()
    {
        return GetFloorFromPosition(player.transform.position);
    }

    private int GetFloorFromPosition(Vector2 position)
    {
        int floorIndex = 0;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (Mathf.Abs(position.y - waypoints[i].position.y) < 1.0f)
            {
                floorIndex = i;
                break;
            }
        }
        return floorIndex;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
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
}
