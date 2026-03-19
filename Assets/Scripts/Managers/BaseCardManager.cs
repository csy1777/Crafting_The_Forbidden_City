using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseCardManager : SingleTon<BaseCardManager>
{
    public GameObject startPos;
    public GameObject endPos;
    public float intervalGenerationTime;
   
    public List<BaseCard> baseCards =new List<BaseCard>();
    

    private void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(SpawnBaseCard());
    }
    

    IEnumerator SpawnBaseCard()
    {
        yield return new WaitForSeconds(.5f);
        while (!GameManager.Instance.isGameOver)
        {
            GetRandomCard();
            yield return new WaitForSeconds(intervalGenerationTime);
        }
    }

    private void GetRandomCard()
    {
        int index=Random.Range(0, baseCards.Count);
        BaseCard baseCard = Instantiate(baseCards[index], transform);
        baseCard.endPos=endPos.transform;
        baseCard.transform.position = startPos.transform.position; 
    }
}
