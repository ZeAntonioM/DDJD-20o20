using UnityEngine;

public class PlayerStairs : MonoBehaviour
{
    [SerializeField] private float stairClimbSpeed = 3f;
    [SerializeField] private GameObject upperFloor;
    
    private Collider2D currentUpperFloorCollider;
    private Rigidbody2D rb;
    private Animator animator;
    private float vertical;
    private bool isOnStairs = false;
    private bool isClimbing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");

        if (isOnStairs && vertical > 0f)
        {
            isClimbing = true;
            animator.SetBool("isClimbing", true);
            animator.SetBool("isDescending", false);
        }
        else if (isOnStairs && vertical < 0f)
        {
            isClimbing = true;
            animator.SetBool("isClimbing", false);
            animator.SetBool("isDescending", true);
        }
        else if (isOnStairs && vertical == 0)
        {
            isClimbing = false;
            animator.SetBool("isClimbing", false);
            animator.SetBool("isDescending", false);
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * stairClimbSpeed);
            if(currentUpperFloorCollider != null)
            {
                currentUpperFloorCollider.isTrigger = true;
            }
        }
        else
        {
            rb.gravityScale = 1f;
            if(currentUpperFloorCollider != null)
            {
                currentUpperFloorCollider.isTrigger = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stairs"))
        {
            isOnStairs = true;
            currentUpperFloorCollider = other.transform.Find("UpperFloor").GetComponent<Collider2D>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Stairs"))
        {
            isOnStairs = false;
            isClimbing = false;
            animator.SetBool("isClimbing", false);
            animator.SetBool("isDescending", false);
            if(currentUpperFloorCollider != null)
            {
                currentUpperFloorCollider.isTrigger = false;
                currentUpperFloorCollider = null;
            }
        }
    }
}
