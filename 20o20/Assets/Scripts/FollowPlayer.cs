using UnityEngine;

public class followPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [Header("Transition Control")]
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector3 velocity;

    private double lastY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastY = player.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y > lastY)
        {
            Vector3 newPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
            lastY = player.position.y;
        }
    }
}
