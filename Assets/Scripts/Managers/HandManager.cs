using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : SingleTon<HandManager>
{
    public Card currentCard;
    public float checkRadius=1;
    public LayerMask checkLayer;
    public AdvancedCard woodenComponent;
    public AdvancedCard stoneComponent;
    public AdvancedCard tileComponent;
    public AdvancedCard decorativeComponent;
    private Vector3 handPos;
    private Collider2D checkCollider;
    private AdvancedCardType advancedCardType=AdvancedCardType.none;
    
    private BaseCard handCard;
    private BaseCard cellCard;
    
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
                            if (currentCard.cardType == CardType.BaseType&&
                                cell.currentCard.cardType == CardType.BaseType)
                            {
                                handCard=currentCard.GetComponent<BaseCard>();
                                cellCard=cell.currentCard.GetComponent<BaseCard>();
                                if (handCard != null && cellCard != null)
                                {
                                    if (handCard.materialCardType == MaterialCardType.none &&
                                        cellCard.toolCardType == ToolCardType.none)
                                    {
                                        advancedCardType = GetAdvancedCardType(cellCard,handCard);
                                    }
                                    else if (handCard.toolCardType == ToolCardType.none &&
                                             cellCard.materialCardType == MaterialCardType.none)
                                    {
                                        advancedCardType = GetAdvancedCardType(handCard,cellCard);
                                    }
                                    else
                                    {
                                        advancedCardType = AdvancedCardType.none;
                                    }
                                    InstantiateAdvancedCard(advancedCardType,handCard,cellCard);
                                }
                            }
                            else
                            {
                                if (currentCard.cardType == CardType.BaseType&&
                                    cell.currentCard.cardType == CardType.BaseType)
                                {
                                    Debug.Log("手上的卡片和卡槽里的都是基础卡");
                                }
                                else if(currentCard.cardType == CardType.AdvancedType&&
                                        cell.currentCard.cardType == CardType.AdvancedType)
                                {
                                    Debug.Log("手上的卡片和卡槽里的都是高级卡");
                                }
                                else
                                {
                                    Debug.Log("手上的卡片和卡槽里的一个是基础卡,一个是高级卡");
                                }
                            }
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
    private AdvancedCardType GetAdvancedCardType(BaseCard materialCard,BaseCard toolCard)
    {
        if (materialCard.materialCardType == MaterialCardType.wood &&
            toolCard.toolCardType == ToolCardType.saw)
        {
            return AdvancedCardType.woodenComponent;
            Debug.Log("合成了木构件");
        }
        else if (materialCard.materialCardType == MaterialCardType.stone &&
            toolCard.toolCardType == ToolCardType.chiselAndhammer)
        {
            return AdvancedCardType.stoneComponent;
            Debug.Log("合成了石构件");
        }
        else if (materialCard.materialCardType == MaterialCardType.clay &&
            toolCard.toolCardType == ToolCardType.kilnFire)
        {
            return AdvancedCardType.tileComponent;
            Debug.Log("合成了瓦构件");
        }
        else if (materialCard.materialCardType == MaterialCardType.paint &&
            toolCard.toolCardType == ToolCardType.goldPowder)
        {
            return AdvancedCardType.decorativeComponent;
            Debug.Log("合成了装饰构件");
        }
        else
        {
            return AdvancedCardType.none;
            Debug.Log("材料类型和工具类型没匹配");
        }
    }

    private void InstantiateAdvancedCard(AdvancedCardType cardType,BaseCard handCard,BaseCard cellCard)
    {
        if (cardType == AdvancedCardType.woodenComponent)
        {
            AdvancedCard obj=Instantiate(woodenComponent);
            currentCard = obj;
        }
        else if (cardType == AdvancedCardType.stoneComponent)
        {
            AdvancedCard obj=Instantiate(stoneComponent);
            currentCard = obj;
            
        }
        else if (cardType == AdvancedCardType.tileComponent)
        {
            AdvancedCard obj=Instantiate(tileComponent);
            currentCard = obj;
        }
        else if (cardType == AdvancedCardType.decorativeComponent)
        {
            AdvancedCard obj=Instantiate(decorativeComponent);
            currentCard = obj;
        }
        else if (cardType == AdvancedCardType.none)
        {
            Debug.Log("NO Match,Delete");
        }
        Destroy(handCard.gameObject);
        Destroy(cellCard.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (currentCard != null)
            Gizmos.DrawWireSphere(currentCard.transform.position, checkRadius);
    }
    
}
