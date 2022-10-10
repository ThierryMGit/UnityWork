using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject leaderboardUIPrefab;

    public void StartGame()
    {
          SceneManager.LoadScene("Tetris");
    }

    public void showLeaderboard()
    {
       Instantiate(leaderboardUIPrefab, transform);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
