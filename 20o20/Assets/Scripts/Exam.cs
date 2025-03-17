using UnityEngine;

public class Exam : MonoBehaviour
{

    private GameController gameController;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = FindFirstObjectByType<GameController>();

        if (gameController == null)
        {
            Debug.LogError("GameController not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(gameController != null){
                gameController.GameWin();
            }
        }
    }
}
