using UnityEngine;

public class RedbullPowerup : MonoBehaviour
{

    [Header("Powerup Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CapsuleCollider2D capsuleCollider2D;

    [Header("Powerup Settings")]
    [SerializeField] private float timer = 10;    
    [SerializeField] private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (spriteRenderer.enabled == false)
        {
            timer -= Time.deltaTime;
            Debug.Log(timer);
            if (timer <= 0)
            {
                spriteRenderer.enabled = true;
                capsuleCollider2D.enabled = true;
                timer = 10;
                player.GetComponent<playerMovement>().DefaultSpeed();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Disable the powerup
            spriteRenderer.enabled = false;
            capsuleCollider2D.enabled = false;

            // Increase the player speed
            player.GetComponent<playerMovement>().SpeedUp();

        }
    }

}
