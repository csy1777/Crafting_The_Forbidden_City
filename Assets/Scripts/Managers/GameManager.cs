using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public bool isGameOver = false;
    public UnityAction onGameOver;

    private void Start()
    {
        onGameOver += GameOver;
        onGameOver += LoadNextGameScene;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            onGameOver?.Invoke();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        isGameOver = true;
    }
    public void LoadNextGameScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
