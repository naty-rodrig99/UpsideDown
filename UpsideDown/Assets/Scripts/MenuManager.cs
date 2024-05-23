using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject gameOverMenu;

    AudioSource[] audios;

    // Start is called before the first frame update
    void Start()
    {

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
    }

    public void RestartForGameOver()
    {
        gameOverMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
