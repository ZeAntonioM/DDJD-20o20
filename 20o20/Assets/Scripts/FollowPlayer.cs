using System;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Transition Control")]
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector3 velocity;


    private float yoffset = 0.0f;

    void Start()
    {
        yoffset = transform.position.y - player.position.y;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y + yoffset, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
