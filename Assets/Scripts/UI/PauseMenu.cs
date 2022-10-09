using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void PauseGame()
    {
        Game.gamePaused = true;
        Time.timeScale = 0;
    }
     
    public void ResumeGame()
    {
        Game.gamePaused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        Game.gamePaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void BackToMainMenu()
    {
        Game.gamePaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
