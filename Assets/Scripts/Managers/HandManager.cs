using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : SingleTon<HandManager>
{
    public Card currentCard;
    public float checkRadius=1;
    public LayerMask checkLayer;
    private Vector3 handPos;
    private Collider2D checkCollider;
    
    private void Update()
    {
        FollowCursor();
        
        //如果手上有卡片,点击鼠标右键就能删除
        if (Input.GetMouseButtonDown(1))
        {
            if (currentCard != null)
            {
                Destroy(currentCard.gameObject);
                ClearHand();
            }
        }
        
        if (FindCell()&&Input.GetMouseButtonDown(0))
        {
            if (checkCollider != null)
            {
                Cell cell = checkCollider.GetComponent<Cell>();
                {
                    if (cell != null)
                    {
                        bool success=cell.AddCard(currentCard);
                        if (success)
                        {
                            currentCard.canClick = false;
                            ClearHand();
                            checkCollider = null;
                            Debug.Log("已成功放入");
                        }
                        else
                        {
                           Debug.Log("有卡片了");
                        }
                    }
                }
            }
        }
    }

    private void FollowCursor()
    {
        if (currentCard == null)
        {
            return;
        }

        handPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        handPos.z = currentCard.transform.position.z;
        currentCard.transform.position = handPos;
    }

    public void SetCurrentCard(Card Card)
    {
        currentCard=Card;
    }
    public void ClearHand()
    {
        currentCard = null;
    }
    private bool FindCell()
    {
        if (currentCard != null)
        {
            Collider2D cellCollider2D = Physics2D.OverlapCircle(currentCard.transform.position, checkRadius, checkLayer );
            if (cellCollider2D != null)
            {
                checkCollider = cellCollider2D;
                return true;
            }

            return false;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (currentCard != null)
            Gizmos.DrawWireSphere(currentCard.transform.position, checkRadius);
    }
    
}
