using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject linkedDoor;
    [SerializeField] private GameObject player;
    [SerializeField] private bool isElevator = false;
    [SerializeField] private float openingTimer = 0.25f;


    private bool isPlayerColliding = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private SpriteRenderer playerRenderer;
    private Animator doorAnimator;
    private Animator linkedDoorAnimator;
    private Animator playerAnimator;
    private PlayerStatus playerStatus;
    private bool inputsDisabled = false;

    void Start()
    {
        InitializeComponents();
    }

    void Update()
    {
        if(inputsDisabled) return;
        if (isPlayerColliding && Input.GetKeyDown(KeyCode.W))
        {
            playerStatus.SetInvisibility(true, false);
            PlayOpeningAnimation();
            StartCoroutine(WaitForOpeningTimer());

        }
    }

    private IEnumerator WaitForOpeningTimer()
    {
        inputsDisabled = true;
        // Disable player physics
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero; // Stop movement
            playerRb.bodyType = RigidbodyType2D.Kinematic; // Disable physics
        }

        // Wait for the door opening timer
        yield return new WaitForSeconds(openingTimer);

        // Start climbing animation
        playerAnimator.SetBool("isClimbing", true);

        // Move player to the door's Y position
        Vector3 startPosition = player.transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, transform.position.y, startPosition.z);
        Vector3 positionDifference = targetPosition - startPosition;
        float transitionDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            player.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the player reaches the target position
        player.transform.position = targetPosition;

        // Stop climbing animation
        playerAnimator.SetBool("isClimbing", false);

        ClosingAnimation();
        HidePlayer();

        Vector3 door1 = player.transform.position;
        Vector3 door2 = new Vector3(linkedDoor.transform.position.x, linkedDoor.transform.position.y, linkedDoor.transform.position.z);
        float elapsedTime2 = 0f;
        while (elapsedTime2 < 3f)
        {
            player.transform.position = Vector3.Lerp(door1, door2, elapsedTime2 / 3f);
            elapsedTime2 += Time.deltaTime;
            yield return null;
        }

        // Ensure the player reaches the target position
        player.transform.position = door2;

        PlayOpeningAnimation(false);
        yield return new WaitForSeconds(openingTimer);
        ShowPlayer();
        playerAnimator.SetBool("isDescending", true);

        float elapsedTime3 = 0f;
        while (elapsedTime3 < transitionDuration)
        {
            player.transform.position = Vector3.Lerp(door2, door2-positionDifference, elapsedTime3 / transitionDuration);
            elapsedTime3 += Time.deltaTime;
            yield return null;
        }

        player.transform.position = (door2-positionDifference);

        playerAnimator.SetBool("isDescending", false);
        playerStatus.SetInvisibility(false, false);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -5);

        ClosingAnimation(false);
        if (playerRb != null)
        {
            playerRb.bodyType = RigidbodyType2D.Dynamic;
        }
        yield return new WaitForSeconds(1f);
        inputsDisabled = false;
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

    private void InitializeComponents()
    {
        playerRenderer = player.GetComponentInChildren<SpriteRenderer>();
        doorAnimator = isElevator ? GetChildAnimator(transform, "Elevator") : GetComponent<Animator>();
        linkedDoorAnimator = isElevator ? GetChildAnimator(linkedDoor.transform, "Elevator") : linkedDoor.GetComponent<Animator>();
        playerAnimator = player.GetComponent<Animator>();
        playerStatus = player.GetComponent<PlayerStatus>();

        if (doorAnimator == null || linkedDoorAnimator == null)
        {
            Debug.LogWarning("Animator components are missing on the door or linked door.");
        }
    }

    private Animator GetChildAnimator(Transform parent, string childName)
    {
        Transform child = parent.Find(childName);
        return child != null ? child.GetComponent<Animator>() : null;
    }

    private void PlayOpeningAnimation(bool currentDoor = true)
    {
        if (currentDoor)
        {
            doorAnimator.SetBool("Opening", true);
            doorAnimator.SetBool("Open", true);
        }
        else
        {
            linkedDoorAnimator.SetBool("Opening", true);
            linkedDoorAnimator.SetBool("Open", true);
        }

    }

    private void ClosingAnimation(bool currentDoor = true)
    {
        if (currentDoor)
        {
            doorAnimator.SetBool("Opening", false);
            doorAnimator.SetBool("Open", false);
        }
        else
        {
            linkedDoorAnimator.SetBool("Opening", false);
            linkedDoorAnimator.SetBool("Open", false);
        }
    }

    private void HidePlayer()
    {
        if (playerRenderer != null) playerRenderer.enabled = false;
    }

    private void ShowPlayer()
    {
        if (playerRenderer != null) playerRenderer.enabled = true;
    }
}