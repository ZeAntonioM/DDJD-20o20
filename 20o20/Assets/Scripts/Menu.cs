using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class Menu : MonoBehaviour
{
    // Reference to UI elements to show scores
    [SerializeField] private GameObject scoresPanel;
    [SerializeField] private TextMeshProUGUI scoresText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (scoresPanel != null)
            scoresPanel.SetActive(false);
    }

    public void ShowScores()
    {
        List<RunData> sortedRuns = LoadAndSortRuns();
        DisplayScores(sortedRuns);
        
        if (scoresPanel != null)
            scoresPanel.SetActive(true);
        
        gameObject.SetActive(false);
    }
    
    private List<RunData> LoadAndSortRuns()
    {
        string directoryPath = Path.Combine(Application.dataPath, "Runs");
        string filePath = Path.Combine(directoryPath, "game_runs.json");
        
        if (!File.Exists(filePath))
        {
            Debug.Log("No Runs file found!");
            return new List<RunData>();
        }
        
        string json = File.ReadAllText(filePath);
        RunsData runsData = JsonUtility.FromJson<RunsData>(json);
        
        List<RunData> sortedRuns = runsData.runs
            .OrderBy(r => r.timeTaken)
            .ThenByDescending(r => r.score)
            .ToList();
        
        return sortedRuns;
    }
    
    private void DisplayScores(List<RunData> runs)
    {
        if (scoresText == null)
        {
            Debug.LogError("ScoresText reference is missing!");
            return;
        }
        
        // Build the scores text
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("HIGH SCORES\n");
        
        if (runs.Count == 0)
        {
            sb.AppendLine("No scores recorded yet!");
        }
        else
        {
            sb.AppendLine("RANK    SCORE      TIME");
            sb.AppendLine("------------------------");
            
            for (int i = 0; i < Mathf.Min(runs.Count, 10); i++)  // Show top 10 scores
            {
                RunData run = runs[i];
                string formattedTime = FormatTime(run.timeTaken);
                sb.AppendLine($"{i+1,2}.     {run.score,6}     {formattedTime}");
            }
        }
        
        scoresText.text = sb.ToString();
    }
    
    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60F);
        int secs = Mathf.FloorToInt(seconds % 60F);
        return string.Format("{0:00}:{1:00}", minutes, secs);
    }

    public void GoToGame()
    {
        // Load the scene with the name "Level1"
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
    
    public void BackToMainMenu()
    {
        if (scoresPanel != null)
            scoresPanel.SetActive(false);
            gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }
}