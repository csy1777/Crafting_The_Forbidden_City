using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All_Hall_over : MonoBehaviour
{
    // Start is called before the first frame update
    public void Btn_start()
    {
        CraftingSystem.Instance.CraftItem("all_halls_complete");
    }
}
