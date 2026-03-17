using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingManager : SingleTon<BuildingManager>
{
    public int needWoodComponent;
    public int needStoneComponent;
    public int needTileComponent;
    public int needDecorativeComponent;
    public int SceneIndex;

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
    }
}
