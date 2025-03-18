using TMPro;
using UnityEngine;

public class TimeLeft : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TMP;
    
    [SerializeField] public float timeToComplete = 300.0f;
    private float timeLeft;
    private GameController gameController;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeLeft = timeToComplete;
        int minutes = Mathf.FloorToInt(timeLeft / 60F);
        int seconds = Mathf.FloorToInt(timeLeft % 60F);
        TMP.text = "Time Left: " + string.Format("{0:00}m {1:00}s", minutes, seconds);
        gameController = FindFirstObjectByType<GameController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timeLeft / 60F);
            int seconds = Mathf.FloorToInt(timeLeft % 60F);
            TMP.text = "Time Left: " + string.Format("{0:00}m {1:00}s", minutes, seconds);
        }
        else
        {
            TMP.text = "Time's up!";
            if (gameController != null)
            {
                gameController.GameOver();
            }
        }
    }

    // New method to return time left in seconds
    public float GetTimeLeftInSeconds()
    {
        return timeLeft;
    }

    // New method to return time taken in seconds
    public float GetTimeTakenInSeconds()
    {
        return timeToComplete - timeLeft;
    }

    public void StopTimer()
    {
        timeLeft = 0;
    }
}