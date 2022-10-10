using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardUI : MonoBehaviour
{
    const int MaxQuantityScores = 7;

    public Transform scoresTransform;

    public GameObject scorePrefab;

    // Start is called before the first frame update
    void Start()
    {
        buildLeaderBoard(GameObject.Find("ScoreManager").GetComponent<ScoreManager>().leaderboard);
    }

    void buildLeaderBoard(Leaderboard leaderboardDatas)
    {
        for(int i = 0; i < Mathf.Min(leaderboardDatas.scoreDatas.Count, MaxQuantityScores); i++) {
            GameObject scoreUI = Instantiate(scorePrefab, scoresTransform);
            scoreUI.GetComponent<ScoreUI>().setDatas(leaderboardDatas.scoreDatas[i]);
        }
    }

    public void back()
    {
        Destroy(this.gameObject);
    }
}
