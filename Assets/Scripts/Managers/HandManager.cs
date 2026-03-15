using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : SingleTon<HandManager>
{
    public BaseCard currentBaseCard;

    private Vector3 handPos;
    private void Update()
    {
        FollowCursor();
    }

    private void FollowCursor()
    {
        if (currentBaseCard == null)
        {
            return;
        }

        handPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        handPos.z = currentBaseCard.transform.position.z;
        currentBaseCard.transform.position = handPos;
    }

    public void SetCurrentBaseCard(BaseCard baseCard)
    {
        currentBaseCard=baseCard;
    }
    public void ClearCard()
    {
        currentBaseCard = null;
    }
}
