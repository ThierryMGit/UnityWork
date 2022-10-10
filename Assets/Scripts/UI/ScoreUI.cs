using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TMPro.TMP_Text pseudoTextValue;
    public TMPro.TMP_Text scoreTextValue;

    public void setDatas(ScoreData scoreData)
    {
        pseudoTextValue.text = scoreData.name;
        scoreTextValue.text = scoreData.score.ToString();
    }
}
