using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public TMPro.TMP_Text scoreTextValue;

    public GameObject noScoreUI;
    public GameObject saveScoreUI;

    public GameObject leaderboardUIPrefab;

    private string _pseudo;

    private int _score;

    private ScoreManager _scoreManagerScript;

    void Awake()
    {
         _scoreManagerScript = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    void Start()
    {
        Game.gamePaused = true;
        Time.timeScale = 0;
    }

    public void setScore(int score)
    {
        _score = score;
        scoreTextValue.text = score.ToString();

        if(score > 0 && _scoreManagerScript.isScoreNeedToBeSaved(score)) {
            saveScoreUI.SetActive(true);
        } else {
            noScoreUI.SetActive(true);
        }
    }

    public void saveScore()
    {
       _scoreManagerScript.SaveScore(_pseudo, _score);
       Instantiate(leaderboardUIPrefab, transform.parent);

       saveScoreUI.SetActive(false);
       noScoreUI.SetActive(true);
    }

    public void setPseudo(string pseudo)
    {
        _pseudo = pseudo;

        GameObject SaveScoreButton  = saveScoreUI.transform.Find("SaveScoreButton").gameObject;
        SaveScoreButton.GetComponent<Button>().interactable = !string.IsNullOrEmpty(_pseudo);
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
