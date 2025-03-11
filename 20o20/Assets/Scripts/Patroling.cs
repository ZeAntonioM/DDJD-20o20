using UnityEngine;

public class Patroling : MonoBehaviour
{
    private Transform PointA;
    private Transform PointB;
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 1;
    [SerializeField] private float idleTime = 2f; // Time to wait at each point
    private bool foundPlayer = false;

    private Transform currentPoint;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isIdle = false;
    private float idleTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Transform parent = transform.parent;

        if(parent != null){
            PointA = parent.Find("PointA");
            PointB = parent.Find("PointB");
            
            if(PointA != null && PointB != null){
                currentPoint = PointB;
            } else {
                Debug.LogError("Patroling: PointA or PointB not found");
            }
        }
    }

    void Update()
    {
        if(foundPlayer){
            rb.linearVelocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
        } else if (isIdle) {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTime) {
                isIdle = false;
                idleTimer = 0f;
                currentPoint = (currentPoint == PointA) ? PointB : PointA;
            }
        } else {
            PatrolBehavior();
        }
    }

    private void PatrolBehavior(){
        Vector2 direction = currentPoint.position - transform.position;

        if (currentPoint == PointA){
            rb.linearVelocity = new Vector2(-speed, 0);
            spriteRenderer.flipX = true;
        } else {
            rb.linearVelocity = new Vector2(speed, 0);
            spriteRenderer.flipX = false;
        }

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f){
            isIdle = true;
            rb.linearVelocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject == player){
            PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
            Debug.Log("Player found");
            if(playerStatus == null || !playerStatus.isInvisible){
                foundPlayer = true;
            }
        }
    }
}