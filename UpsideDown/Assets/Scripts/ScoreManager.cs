using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int scoreMax = 130;
    public int currentScore = 0;

    /*public Image scoreIcon;
    public TextMeshProUGUI scoreText;

    public Sprite scoreType1;
    public Sprite scoreType2;
    public Sprite scoreType3;
    public int silverScore;
    public int goldScore;*/

    public Image progressBarFill;

    // Start is called before the first frame update
    void Start()
    {
        //UpdateScoreIconAndText();
        UpdateProgressBar();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*void UpdateScoreIconAndText()
    {
        if (currentScore >= silverScore)
        {
            scoreIcon.sprite = scoreType2;
        }
        else if (currentScore >= goldScore)
        {
            scoreIcon.sprite = scoreType3;
        }
        else
        {
            scoreIcon.sprite = scoreType1;
        }

        scoreText.text = currentScore.ToString();
    }*/

    public void ModifyScore(int amount)
    {
        currentScore += amount;
        //UpdateScoreIconAndText();
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        // Calculate the fill amount (between 0 and 1)
        float fillAmount = (float)currentScore / scoreMax;
        progressBarFill.fillAmount = fillAmount;
    }

}
