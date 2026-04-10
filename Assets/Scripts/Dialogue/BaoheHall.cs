using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaoheHall : MonoBehaviour
{
    void Start()
    {
        CraftingSystem.Instance.CraftItem("baohe_hall_material");
    }
}
