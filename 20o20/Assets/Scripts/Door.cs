using UnityEngine;
using System.Collections;

public class door : MonoBehaviour
{
    [SerializeField] private GameObject linkedDoor;
    [SerializeField] private GameObject player;
    [SerializeField] private float transitionSpeed = 0.5f;
    
    private bool isPlayerColliding = false;
    private bool isTransitioning = false;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float transitionProgress = 0f;
    private SpriteRenderer playerRenderer;
    
    void Start()
    {
        playerRenderer = player.GetComponent<SpriteRenderer>();
        
        if (playerRenderer == null)
        {
            playerRenderer = player.GetComponentInChildren<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (!isTransitioning && isPlayerColliding && Input.GetKeyDown(KeyCode.W))
        {
            StartTransition();
        }
        
        if (isTransitioning)
        {
            ContinueTransition();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            isPlayerColliding = false;
        }
    }

    private void StartTransition()
    {
        isTransitioning = true;
        startPosition = player.transform.position;
        targetPosition = new Vector3(linkedDoor.transform.position.x, linkedDoor.transform.position.y, player.transform.position.z);
        transitionProgress = 0f;

        HidePlayer();
    }

    private void HidePlayer()
    {
        if (playerRenderer != null)
        {
            playerRenderer.enabled = false;
        }
    }

    private void ShowPlayer()
    {
        if (playerRenderer != null)
        {
            playerRenderer.enabled = true;
        }
    }

    private void ContinueTransition()
    {
        transitionProgress += Time.deltaTime * transitionSpeed;
        
        if (transitionProgress >= 1f)
        {
            player.transform.position = targetPosition;
            isTransitioning = false;
            
            ShowPlayer();
        }
        else
        {
            // Continue moving
            player.transform.position = Vector3.Lerp(startPosition, targetPosition, transitionProgress);
        }
    }
}