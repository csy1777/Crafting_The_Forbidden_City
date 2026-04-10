using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cell : MonoBehaviour
{
   public Card currentCard;
   public bool AddCard(Card Card)
   {
      if(currentCard!=null)return false;
      currentCard = Card;
      currentCard.transform.position = transform.position;
      return true;
   }
   public void ClearCard()
   {
      currentCard = null;
   }
}


