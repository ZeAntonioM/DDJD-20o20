using UnityEngine;
using UnityEngine.UIElements;

public class CafeShop : MonoBehaviour
{

    [SerializeField] private GameObject Player;
    [SerializeField] private int points = 1000;

    void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject == Player)
        {
            
            Player.GetComponent<PlayerStatus>().AddPoints(points);
            Debug.Log("Player has " + Player.GetComponent<PlayerStatus>().GetPoints() + " points");
            Destroy(gameObject);

        }

    }

}
