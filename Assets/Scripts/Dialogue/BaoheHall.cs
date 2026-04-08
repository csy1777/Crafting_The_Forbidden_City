using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaoheHall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CraftingSystem.Instance.CraftItem("baohe_hall_material");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
