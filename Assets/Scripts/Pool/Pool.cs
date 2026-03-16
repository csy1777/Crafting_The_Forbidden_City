using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class Pool 
{
    public GameObject prefab;
    public ObjectPool<GameObject> pool;

    public Pool(GameObject prefab)
    {
        this.prefab = prefab;

        // 初始化官方对象池
        pool = new ObjectPool<GameObject>(
            CreateFunc,
            ActionOnGet,
            ActionOnRelease,
            ActionOnDestroy,
            true, 10, 1000
        );
    }

    private GameObject CreateFunc()
    {
        var obj=GameObject.Instantiate(prefab);
        return obj;
    }
    private void ActionOnGet(GameObject obj)
    {
        obj.SetActive(true);
    }
    private void  ActionOnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void ActionOnDestroy(GameObject obj)
    {
        GameObject.Destroy(obj);
    }

    //自定义的利用和回收的方法
    public  GameObject GetPrefab(Vector3 pos)
    {
        GameObject go=pool.Get();
        go.transform.position=pos;
        return go;
    }

    public  void ReleasePrefab(GameObject obj)
    {
        pool.Release(obj);
        InitPrefab(obj);
    }

    public  void InitPrefab(GameObject obj)
    {
        obj.transform.DOKill();
    }
}