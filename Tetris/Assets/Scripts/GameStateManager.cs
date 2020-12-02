using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameObject gameOverScreen;

    public void Start()
    {
        gameOverScreen.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOverScreen.SetActive(true);
    }
}
