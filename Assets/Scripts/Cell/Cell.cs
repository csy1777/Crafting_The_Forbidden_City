using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cell : MonoBehaviour
{
   public BaseCard currentBaseCard;
   

   public bool AddBaseCard(BaseCard baseCard)
   {
      if(currentBaseCard!=null)return false;
      currentBaseCard = baseCard;
      currentBaseCard.transform.position = transform.position;
      return true;
   }
   
}


