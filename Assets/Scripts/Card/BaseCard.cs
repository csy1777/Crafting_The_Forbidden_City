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
   saw,//锯子
   chiselAndhammer,//凿子和锤子
   kilnFire,//窑火
   goldPowder,//金粉
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
      HandManager.Instance.SetCurrentCard(this);
      canMove = false;
      Debug.Log(gameObject.name);
      if (currentCell)
      {
         currentCell.currentCard = null;
         currentCell = null;
      }
   }
}
