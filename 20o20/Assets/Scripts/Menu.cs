using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void GoToGame()
    {
        // Load the scene with the name "Level1"
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }

/*
    void SeeHighScores()
    {
        // Load the scene with the name "HighScores"
        UnityEngine.SceneManagement.SceneManager.LoadScene("HighScores");
    }
*/

}
