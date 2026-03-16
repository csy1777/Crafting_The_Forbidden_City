using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : SingleTon<HandManager>
{
    public BaseCard currentBaseCard;
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
            if (currentBaseCard != null)
            {
                Destroy(currentBaseCard.gameObject);
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
                        bool success=cell.AddBaseCard(currentBaseCard);
                        if (success)
                        {
                            currentBaseCard.canClick=false;
                            ClearHand();
                            checkCollider = null;
                        }
                        else
                        {
                            Debug.Log(currentBaseCard);
                            Debug.Log(cell.currentBaseCard);
                        }
                    }
                }
            }
        }
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
    public void ClearHand()
    {
        currentBaseCard = null;
    }
    private bool FindCell()
    {
        if (currentBaseCard != null)
        {
            Collider2D cellCollider2D = Physics2D.OverlapCircle(currentBaseCard.transform.position, checkRadius, checkLayer );
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
        if (currentBaseCard != null)
            Gizmos.DrawWireSphere(currentBaseCard.transform.position, checkRadius);
    }
}
