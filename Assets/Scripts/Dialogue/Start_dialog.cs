using UnityEngine;

public class Start_dialog : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           //初始进入界面开始对话
            CraftingSystem.Instance.CraftItem("start_dialog");
        }
    }
}