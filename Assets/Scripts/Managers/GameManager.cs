using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public static bool isGameOver = false;
    public UnityAction onGameOver;

    private void Start()
    {
        onGameOver += TestGameOver;
    }

    public void TestGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
    }
}
