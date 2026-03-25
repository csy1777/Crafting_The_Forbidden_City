using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingManager : SingleTon<BuildingManager>
{
    public int needWoodComponent;
    public int needStoneComponent;
    public int needTileComponent;
    public int needDecorativeComponent;
    
    public SpriteRenderer roofSprite;
    public SpriteRenderer mainBodySprite;
    public SpriteRenderer platformBaseSprite;
    
    public  int currentWoodComponent=0;
    public  int currentStoneComponent=0;
    public  int currentTileComponent=0;
    public  int currentDecorativeComponent=0;
    public  int mainBodyPoint = 0;
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
        UpdateSpriteColors();
    }

    private void UpdateSpriteColors()
    {
        float roofPercentage = (float)currentTileComponent / needTileComponent;
        float mainBodyPercentage = (float)mainBodyPoint / (needWoodComponent+needDecorativeComponent);
        float platformPercentage = (float)currentStoneComponent / needStoneComponent;
        
        roofSprite.color = Color.Lerp(Color.black, Color.white, roofPercentage);
        mainBodySprite.color = Color.Lerp(Color.black, Color.white, mainBodyPercentage);
        platformBaseSprite.color = Color.Lerp(Color.black, Color.white, platformPercentage);
    }

}

