using UnityEngine;

public class CardCollector : MonoBehaviour
{
    
    [SerializeField] private GameObject Player;

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject == Player)
        {
            
            Player.GetComponent<PlayerStatus>().SetCard(true);
            Debug.Log("Player has card: " + Player.GetComponent<PlayerStatus>().hasCard);
            Destroy(gameObject);

        }

    }



}
