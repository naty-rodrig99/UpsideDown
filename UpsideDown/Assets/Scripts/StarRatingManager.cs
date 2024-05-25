using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class StarRatingManager : MonoBehaviour
{
    public int maxHealth = 50;
    public int maxKillCount = 50;
    public int maxBullets = 50;
    public float timer;
    public float bestTime = 300.0f;

    public Image star1;
    public Image star2;
    public Image star3;

    public Sprite fullStar;
    public Sprite halfStar;
    public Sprite emptyStar;

    public HealthManager healthManager;
    public ScoreManager scoreManager;
    public BulletManager bulletManager;

    private bool levelCompleted = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (levelCompleted)
        {
            timer += Time.deltaTime;
        }
    }

    void CalculateStarRating()
    {
        int currentHealth = healthManager.currentHearts;
        int enemyKillCount = scoreManager.currentScore;
        int bulletsRemaining = bulletManager.currentBullets;

        float normalizedHealth = (float)currentHealth / maxHealth;
        float normalizedKills = (float)enemyKillCount / maxKillCount;
        float normalizedBullets = (float)bulletsRemaining / maxBullets;
        float normalizedTime = 0;


        float healthWeight = 0.25f;
        float killWeight = 0.25f;
        float bulletWeight = 0.25f;
        float timeWeight = 0f;

        if (levelCompleted)
        {
            normalizedTime = Mathf.Clamp01(1.0f - (timer / bestTime));
            timeWeight = 0.25f;
        }

        float finalScore = (normalizedHealth * healthWeight) +
                           (normalizedKills * killWeight) +
                           (normalizedBullets * bulletWeight) +
                           (normalizedTime * timeWeight);

        float starRating = finalScore * 3;
        float roundedRating = Mathf.Round(starRating * 2.0f) / 2.0f;

        UpdateStarImages(roundedRating);
    }

    void UpdateStarImages(float starRating)
    {
        star1.sprite = emptyStar;
        star2.sprite = emptyStar;
        star3.sprite = emptyStar;

        if (starRating >= 1) star1.sprite = fullStar;
        if (starRating >= 2) star2.sprite = fullStar;
        if (starRating >= 3) star3.sprite = fullStar;

        if (starRating >= 0.5f && starRating < 1) star1.sprite = halfStar;
        if (starRating >= 1.5f && starRating < 2) star2.sprite = halfStar;
        if (starRating >= 2.5f && starRating < 3) star3.sprite = halfStar;
    }

    public void OnGameEnd()
    {
        CalculateStarRating();
    }

}
