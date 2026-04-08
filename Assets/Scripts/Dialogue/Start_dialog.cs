using UnityEngine;

public class Start_dialog : MonoBehaviour
{
   void Start()
    {
        //初始进入界面开始对话
        CraftingSystem.Instance.CraftItem("start_dialog");
    }
}