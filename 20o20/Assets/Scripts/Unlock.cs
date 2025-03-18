using UnityEngine;

public class Unlock : MonoBehaviour
{
    private PlayerStatus playerStatus;
    [SerializeField] private GameObject door;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStatus = GameObject.FindFirstObjectByType<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerStatus != null && playerStatus.hasCard)
            {
                this.gameObject.SetActive(false); // Deactivate the GameObject
                door.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
    }
}