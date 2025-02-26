using UnityEngine;

public class door : MonoBehaviour
{

    [SerializeField] private GameObject linkedDoor;
    [SerializeField] private GameObject player;
    private bool isPlayerColliding = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerColliding && Input.GetKeyDown(KeyCode.W))
        {
            MovePlayerToLinkedDoor();
        }
        
    }

    
    private void OnTriggerEnter2D(Collider2D collission)
    {
        if (collission.gameObject == player)
        {
            isPlayerColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collission)
    {
        if (collission.gameObject == player)
        {
            isPlayerColliding = false;
        }
    }

    private void MovePlayerToLinkedDoor()
    {
        player.transform.position = new Vector3 (linkedDoor.transform.position.x, linkedDoor.transform.position.y, player.transform.position.z);
    }
}
