using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject gameOverScreen;

    public void Start()
    {
        Time.timeScale = 0.0f;
        FindObjectOfType<SpawnBlock>().enabled = false;
        titleScreen.SetActive(true);
        gameOverScreen.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOverScreen.SetActive(true);
    }

    public void PlayGame()
    {
        Time.timeScale = 1.0f;
        FindObjectOfType<SpawnBlock>().enabled = true;
        titleScreen.SetActive(false);
    }

    public void RetryGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Tetris");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
