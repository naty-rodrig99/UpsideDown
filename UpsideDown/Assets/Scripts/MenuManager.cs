using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject gameOverMenu;
    [SerializeField]
    GameObject startMenu;

    AudioSource[] audios;

    public StarRatingManager starsRatingManager;

    // Start is called before the first frame update
    void Start()
    {
        int gameMode = PlayerPrefs.GetInt("gameMode");

        if (gameMode == 0)
        {
            startMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Enable & Disable the pause menu
    public void Pause()
    {
        audios = FindObjectsOfType<AudioSource>();

        if (pauseMenu.activeInHierarchy)
        {
            Resume();
        }
        else
        {
            Stop();
        }
    }

    public void Stop()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        foreach(AudioSource audio in audios)
        {
            audio.Pause();
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        foreach (AudioSource audio in audios)
        {
            audio.Play();
        }
    }

    public void RestartGame()
    {
        Resume();
        PlayerPrefs.SetInt("gameMode", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        audios = FindObjectsOfType<AudioSource>();

        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;

        foreach (AudioSource audio in audios)
        {
            audio.Pause();
        }

        starsRatingManager.OnGameEnd();

    }

    public void RestartForGameOver()
    {
        gameOverMenu.SetActive(false);
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("gameMode", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
