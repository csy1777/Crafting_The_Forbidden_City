using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum MaterialCardType
{
   wood, stone, clay, paint,none
}

public enum ToolCardType
{
   saw,
   chiselAndhammer,
   kilnFire,
   goldPowder,
   none
}

public class BaseCard : Card
{
   public MaterialCardType materialCardType;
   public ToolCardType toolCardType;
   public Transform endPos;
   public float speed = 1f;
   public bool canMove = true;
   

   void Update()
   {
      if (endPos == null) return;
      if (canMove)
      {
         transform.Translate(Vector3.right * (speed * Time.deltaTime));
         if (Vector3.Distance(transform.position, endPos.position) < 0.1f)
            Destroy(gameObject);
      }

      
   }
   private void OnMouseDown()
   {
      if (!GameManager.Instance.isGameOver)
      {
         if (GetComponent<SpriteRenderer>().sortingOrder != 0)
            return;
            AudioManager.Instance.PlayClip(Config.Card_Click, 1);
            HandManager.Instance.SetCurrentCard(this);
            GetComponent<SpriteRenderer>().sortingOrder = 10;
            canMove = false;
            endPos = null;
            if (currentCell)
            {
               currentCell.currentCard = null;
               currentCell = null;
            }
      }
   }
}
