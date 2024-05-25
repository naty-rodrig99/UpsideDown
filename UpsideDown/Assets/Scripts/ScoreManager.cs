using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int scoreMax = 100;
    public int scoreCurrent = 0;

    public Image scoreIcon;
    public TextMeshProUGUI scoreText;

    public Sprite scoreType1;
    public Sprite scoreType2;
    public Sprite scoreType3;
    public int silverScore;
    public int goldScore;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreIconAndText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateScoreIconAndText()
    {
        if (scoreCurrent >= silverScore)
        {
            scoreIcon.sprite = scoreType2;
        }
        else if (scoreCurrent >= goldScore)
        {
            scoreIcon.sprite = scoreType3;
        }
        else
        {
            scoreIcon.sprite = scoreType1;
        }

        scoreText.text = scoreCurrent.ToString();
    }

    public void ModifyScore(int amount)
    {
        scoreCurrent += amount;
        UpdateScoreIconAndText();
    }




}
