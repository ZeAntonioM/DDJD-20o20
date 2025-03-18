using UnityEngine;

public class BroomPowerup : MonoBehaviour
{

    [Header("Powerup Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CapsuleCollider2D capsuleCollider2D;

    [Header("Powerup Settings")]
    [SerializeField] private float timer = 10;    
    [SerializeField] private GameObject player;
    [SerializeField] private float invisibilityDuration = 5f;

    private bool isInvisibilityActive = false;
    private float invisibilityTimer = 0f;
    private PlayerStatus playerStatus;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(player != null){
            playerStatus = player.GetComponent<PlayerStatus>();
            if(playerStatus == null){
                Debug.LogError("BroomPowerup: PlayerStatus not found");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Handle powerup respawn timer
        if (spriteRenderer.enabled == false)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                spriteRenderer.enabled = true;
                capsuleCollider2D.enabled = true;
                timer = 10;
            }
        }

        if(isInvisibilityActive)
        {
            invisibilityTimer -= Time.deltaTime;
            if(invisibilityTimer <= 0)
            {
                isInvisibilityActive = false;
                if(playerStatus != null){
                    playerStatus.SetInvisibility(false);
                }
            }
            else if(playerStatus != null){
                playerStatus.SetInvisibility(true);
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

            if(playerStatus != null){
                playerStatus.SetInvisibility(true);
                isInvisibilityActive = true;
                invisibilityTimer = invisibilityDuration;
            }
        }
    }

}
