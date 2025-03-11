using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    private float horizontalInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update(){
        // Get the input from the player
        horizontalInput = Input.GetAxis("Horizontal");
        rb.linearVelocityX = horizontalInput * speed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        if (horizontalInput > 0)
            spriteRenderer.flipX = false;
        else if (horizontalInput < 0)
            spriteRenderer.flipX = true;
    }

    public void DefaultSpeed(){
        speed = 5;
    }

    public void SpeedUp(){
        speed = 10;
    }

}
