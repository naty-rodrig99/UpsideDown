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

    static int gameMode = 0;

    // Start is called before the first frame update
    void Start()
    {

        if (gameMode == 0)
        {
            startMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        //else
        //    Time.timeScale = 1f;
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
        gameMode = 1;
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
        gameMode = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
