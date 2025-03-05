using UnityEngine;

public class Patroling : MonoBehaviour
{

    private Transform PointA;
    private Transform PointB;
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 1;
    private bool foundPlayer = false;

    private Transform currentPoint;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Transform parent = transform.parent;

        if(parent != null){
            PointA = parent.Find("PointA");
            PointB = parent.Find("PointB");
            
            if(PointA != null && PointB != null){
                currentPoint = PointB;
            } else {
                Debug.LogError("Patroling: PointA or PointB not found");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(foundPlayer){
            rb.linearVelocity = new Vector2(0, 0);
        } else {
            PatrolBehavior();
        }

    }

    private void PatrolBehavior(){
        Vector2 direction = currentPoint.position - transform.position;

        if (currentPoint == PointA){
            rb.linearVelocity = new Vector2(-speed, 0);
        } else {
            rb.linearVelocity = new Vector2(speed, 0);
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f){
            currentPoint = (currentPoint == PointA) ? PointB : PointA;
        }
    }


    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject == player){
            foundPlayer = true;
        }
    }
}
