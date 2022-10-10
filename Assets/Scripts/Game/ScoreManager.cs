using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gère la sauvegarde et chargement des scores
public class ScoreManager : MonoBehaviour
{
    public static int MaxQuantityScores = 8;

    public Leaderboard leaderboard = new Leaderboard();

    void Awake()
    {
        LoadLeaderboard();
    }

    public void SaveScore(string pseudo, int score)
    {
        //
        ScoreData scoreData = new ScoreData(pseudo, score);

        int indexScore = 0;
        while(indexScore < leaderboard.scoreDatas.Count && score <= leaderboard.scoreDatas[indexScore].score) {
            indexScore++;
        }

        leaderboard.scoreDatas.Insert(indexScore, scoreData);
        SaveLeaderboard();
    }

    public bool isScoreNeedToBeSaved(int score)
    {
        int indexScore = 0;
        while(indexScore < Mathf.Min(leaderboard.scoreDatas.Count, MaxQuantityScores) && score <= leaderboard.scoreDatas[indexScore].score) {
            indexScore++;
        }

        return (indexScore < MaxQuantityScores);
    }

    public void SaveLeaderboard()
    {
        string leaderboardData = JsonUtility.ToJson(leaderboard);
        string filePath = Application.persistentDataPath + "/Leaderboard.json";
        System.IO.File.WriteAllText(filePath, leaderboardData);
    }

    public void LoadLeaderboard()
    {
        string filePath = Application.persistentDataPath + "/Leaderboard.json";

        if(!System.IO.File.Exists(filePath)) {
            return;
        }

        string leaderboardData = System.IO.File.ReadAllText(filePath);
        leaderboard = JsonUtility.FromJson<Leaderboard>(leaderboardData);
    }
}

[System.Serializable]
public class Leaderboard
{
    public List<ScoreData> scoreDatas = new List<ScoreData>();
}

[System.Serializable]
public class ScoreData
{
    public string name;
    public int score;

    public ScoreData(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}


