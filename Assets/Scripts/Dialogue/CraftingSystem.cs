using UnityEngine;
using System.Collections.Generic;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance;  // 单例
    // 物品对话映射表：物品ID → 对话内容（字符串数组）
    // 你可以在下方直接添加或修改所有物品的对话
    private Dictionary<string, string[]> itemDialogues = new Dictionary<string, string[]>()
    {
        // 示例：木剑
        { "wood_sword", new string[]
            {
                "恭喜你合成了一把木剑！",
                "这是你的第一件武器，可以用来战斗了。",
                "继续探索吧！"
            }
        },
        { "start_dialog", new string[]
            {
                "欢迎进入游戏！",
                "这是第一关。",
                "继续探索吧！"
            }
        },
        // 示例：石斧
        { "stone_axe", new string[]
            {
                "你合成了一把石斧。",
                "用它砍树会更快哦。"
            }
        },
        // 示例：铁镐
        { "iron_pickaxe", new string[]
            {
                "铁镐！采矿效率大大提升。"
            }
        },
        // 你可以继续添加更多物品...
        // 格式：{ "物品ID", new string[] { "第一句", "第二句", ... } },
    };
    void Awake()
    {
        // 单例初始化
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 记录已经合成过的物品，用于确保每个物品的对话只触发一次
    private HashSet<string> craftedItems = new HashSet<string>();

    /// <summary>
    /// 合成物品（外部调用此方法）
    /// </summary>
    /// <param name="itemId">合成结果的物品ID，必须与字典中的键一致</param>
    public void CraftItem(string itemId)
    {
        // ========== 在这里编写你的实际合成逻辑 ==========
        // 例如：检查材料、扣除资源、生成物品到背包等
        // =============================================

        // 首次合成判断
        if (!craftedItems.Contains(itemId))
        {
            craftedItems.Add(itemId);

            // 查找该物品是否有对话配置
            if (itemDialogues.TryGetValue(itemId, out string[] lines))
            {
                // 启动对话
                DialogueManager.Instance.StartDialogue(lines);
            }
            // 如果没有配置对话，则什么都不做
        }
    }

    // 可选：手动触发某个物品的对话（用于测试或特殊场合）
    public void ShowDialogueForItem(string itemId)
    {
        if (itemDialogues.TryGetValue(itemId, out string[] lines))
        {
            DialogueManager.Instance.StartDialogue(lines);
        }
    }
}