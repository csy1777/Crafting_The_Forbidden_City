using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingleTon<PoolManager>
{
    public GameObject woodCard;
    public GameObject stoneCard;
    public GameObject clayCard;
    public GameObject paintCard;
    
    public static  Dictionary<string, Pool> pools = new Dictionary<string,Pool>();

    protected override void Awake()
    {
        base.Awake();
        pools.Add("woodCard", new Pool(woodCard));
        pools.Add("stoneCard", new Pool(stoneCard));
        pools.Add("clayCard", new Pool(clayCard));
        pools.Add("paintCard", new Pool(paintCard));
    }
}
