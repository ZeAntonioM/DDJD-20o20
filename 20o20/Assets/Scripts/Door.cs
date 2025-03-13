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
    private Animator doorAnimator;
    private Animator linkedDoorAnimator;
    
    void Start()
    {
        playerRenderer = player.GetComponent<SpriteRenderer>();
        
        if (playerRenderer == null)
        {
            playerRenderer = player.GetComponentInChildren<SpriteRenderer>();
        }

        doorAnimator = GetComponent<Animator>();
        linkedDoorAnimator = linkedDoor.GetComponent<Animator>();
    }

    void Update()
    {
        if (!isTransitioning && isPlayerColliding && Input.GetKeyDown(KeyCode.W))
        {
            if(doorAnimator != null)
            {
                doorAnimator.SetBool("Opening", true);
                doorAnimator.SetBool("Open", true);
                linkedDoorAnimator.SetBool("Opening", true);
                linkedDoorAnimator.SetBool("Open", true);
            }
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

        if(doorAnimator != null)
        {
            doorAnimator.SetBool("Opening", false);
            doorAnimator.SetBool("Open", false);
            linkedDoorAnimator.SetBool("Opening", false);
            linkedDoorAnimator.SetBool("Open", false);
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
            player.transform.position = Vector3.Lerp(startPosition, targetPosition, transitionProgress);
        }
    }
}