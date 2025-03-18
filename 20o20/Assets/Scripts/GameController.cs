using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class RunData
{
    public int score;
    public float timeTaken;
    public string date;
}

[System.Serializable]
public class RunsData
{
    public List<RunData> runs = new List<RunData>();
}

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI timeScoreText;
    [SerializeField] private GameObject gameInterface;
    private PlayerStatus playerStatus;
    private TimeLeft time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
        playerStatus = GameObject.FindFirstObjectByType<PlayerStatus>();
        time = GameObject.FindFirstObjectByType<TimeLeft>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver(){
        loseScreen.gameObject.SetActive(true);
        gameInterface.SetActive(false);
    }

    public void GameWin(){

        // Get player stats
        int finalScore = playerStatus.GetPoints();
        float timeTaken = this.GetTimeTaken();

        // Display scores without time bonus
        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + finalScore.ToString("N0");
            
        if (timeScoreText != null)
            timeScoreText.text = "Time: " + FormatTime(timeTaken);
        
        // Log the results
        Debug.Log("Final Score: " + finalScore);
        Debug.Log("Time Taken: " + timeTaken + " seconds");

        winScreen.SetActive(true);
        gameInterface.SetActive(false);

        SaveRunToJson();
    }

    public float GetTimeTaken()
    {
        return time.GetTimeTakenInSeconds();
    }

    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60F);
        int secs = Mathf.FloorToInt(seconds % 60F);
        return string.Format("{0:00}:{1:00}", minutes, secs);
    }

    private void SaveRunToJson()
    {
        string directoryPath = Path.Combine(Application.dataPath, "Runs");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = Path.Combine(directoryPath, "game_runs.json");
        
        // Create new run data
        RunData newRun = new RunData
        {
            score = playerStatus.GetPoints(),
            timeTaken = this.GetTimeTaken(),
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        
        // Load existing data if available
        RunsData runsData;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            runsData = JsonUtility.FromJson<RunsData>(json);
        }
        else
        {
            runsData = new RunsData();
        }
        
        runsData.runs.Add(newRun);
        
        string updatedJson = JsonUtility.ToJson(runsData, true);
        File.WriteAllText(filePath, updatedJson);
        
        Debug.Log("Run saved to: " + filePath);
    }

    public void GoToMenu()
    {
        // Load the scene with the name "Menu"
        PlayerPrefs.Save();
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        
    }
}