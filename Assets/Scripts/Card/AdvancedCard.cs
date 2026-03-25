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
            AudioManager.Instance.PlayClip(Config.Card_Click,1);
            HandManager.Instance.SetCurrentCard(this);
            GetComponent<SpriteRenderer>().sortingOrder = 10;
            if (currentCell)
            {
                currentCell.currentCard = null;
                currentCell = null;
            }
        }
    }
}
