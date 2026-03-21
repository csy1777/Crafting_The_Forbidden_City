using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingManager : SingleTon<BuildingManager>
{
    private  int needWoodComponent;
    private  int needStoneComponent;
    private  int needTileComponent;
    private  int needDecorativeComponent;
    
    public  int currentWoodComponent=0;
    public  int currentStoneComponent=0;
    public  int currentTileComponent=0;
    public  int currentDecorativeComponent=0;
    public  int SceneIndex;

    private void Start()
    {
        SceneIndex=SceneManager.GetActiveScene().buildIndex;
        switch (SceneIndex)
        {
            case 1:
                needWoodComponent=3;
                needStoneComponent=2;
                needTileComponent=8;
                needDecorativeComponent=1;
                break;
            case 2:
                needWoodComponent=13;
                needStoneComponent=8;
                needTileComponent=5;
                needDecorativeComponent=6;
                break;
            case 3:
                needWoodComponent=2;
                needStoneComponent=4;
                needTileComponent=8;
                needDecorativeComponent=5;
                break;
            case 4:
                needWoodComponent=2;
                needStoneComponent=6;
                needTileComponent=6;
                needDecorativeComponent=8;
                break;
        }
        Debug.Log(needWoodComponent+needStoneComponent+needTileComponent+needDecorativeComponent);
    }

    private void Update()
    {
        if (currentWoodComponent >= needWoodComponent &&
            currentDecorativeComponent >= needDecorativeComponent &&
            currentStoneComponent >= needStoneComponent &&
            currentTileComponent >= needTileComponent)
        {
           GameManager.Instance.onGameOver?.Invoke();
        }
    }
}
