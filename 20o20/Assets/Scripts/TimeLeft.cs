using TMPro;
using UnityEngine;

public class TimeLeft : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TMP;
    
    [SerializeField] public float timeToComplete = 300.0f;
    private float timeLeft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeLeft = timeToComplete;
        int minutes = Mathf.FloorToInt(timeLeft / 60F);
        int seconds = Mathf.FloorToInt(timeLeft % 60F);
        TMP.text = "Time Left: " + string.Format("{0:00}m {1:00}s", minutes, seconds);
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
        }
    }
}
