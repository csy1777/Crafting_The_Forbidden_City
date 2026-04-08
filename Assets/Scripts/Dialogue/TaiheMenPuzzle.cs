using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaiheMenPuzzle : MonoBehaviour
{
    void Start()
    {
        //初始进入界面开始对话
        CraftingSystem.Instance.CraftItem("taihe_gate_material");
    }
}
