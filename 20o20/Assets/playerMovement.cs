using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        // Get the input from the player
        horizontalInput = Input.GetAxis("Horizontal");
        rb.linearVelocityX = horizontalInput * 5;
    }
}
