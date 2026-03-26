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
        onGameOver += TestGameOver;
        onGameOver += LoadNextGameScene;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            onGameOver?.Invoke();
        }
    }

    public void TestGameOver()
    {
        Debug.Log("Game Over");
        isGameOver = true;
    }
    public void LoadNextGameScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
