using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    public float fovAngle = 45f;
    public float detectionRange = 10f;
    public Transform player;
    public Transform fovPoint;
    public float rotationSpeed = 0.1f;
    public float startAngle = 30;
    public float endAngle = 150;

    public float timeToBust = 3f;
    private float detectionTimer = 0f;
    private bool playerDetected = false;

    void start()
    {
        transform.rotation = Quaternion.Euler(0, 0, startAngle);
    }

    void Update()
    {
        DetectPlayer();
        rotateCamera();

        if(playerDetected)
        {
            detectionTimer += Time.deltaTime;
            if(detectionTimer >= timeToBust)
            {
                Debug.Log("Player busted");
            }
        } else
        {
            detectionTimer = 0;
        }
    }

    private void DetectPlayer()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, fovPoint.up);
        RaycastHit2D hit = Physics2D.Raycast(fovPoint.position, directionToPlayer, detectionRange);

        playerDetected = false;

        if (angle < fovAngle / 2)
        {
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                playerDetected = true;
                //Debug.Log("Player detected");
            }
        }
    }

    private void rotateCamera()
    {
        float angle = Mathf.PingPong(Time.time * rotationSpeed, 1) * (endAngle - startAngle) + startAngle;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

}