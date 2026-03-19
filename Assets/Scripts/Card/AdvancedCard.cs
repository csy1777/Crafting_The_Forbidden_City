using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AdvancedCardType
{
    woodenComponent,
    stoneComponent,
    tileComponent,
    decorativeComponent,
    none
}
public class AdvancedCard : Card
{
    public AdvancedCardType advancedCardType;

    private void OnMouseDown()
    {
        if (!GameManager.Instance.isGameOver)
        {
            HandManager.Instance.SetCurrentCard(this);
            if (currentCell)
            {
                currentCell.currentCard = null;
                currentCell = null;
            }
        }
    }
}
