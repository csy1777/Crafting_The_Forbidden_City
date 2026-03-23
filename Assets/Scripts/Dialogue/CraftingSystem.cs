using UnityEngine;
using System.Collections.Generic;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance;  // 单例
    // 物品对话映射表：物品ID → 对话内容（字符串数组）
    // 你可以在下方直接添加或修改所有物品的对话
    private Dictionary<string, string[]> itemDialogues = new Dictionary<string, string[]>()
    {
     
        
        { "start_dialog", new string[]
            {
                   "[老者]阿明，皇上已下旨，命我主持建造紫禁城四座核心建筑：奉天门、奉天殿、华盖殿、谨身殿。",
    "[青年]师父，这是何等荣耀！弟子定当竭尽全力，协助您完成这千秋伟业。",
    "[老者]好！但工程浩大，非一日之功。你需要收集石、木、瓦、装饰四种构件，将它们合成完整的部件，方可逐一建造。",
    "[青年]石构件为基，木构件为骨，瓦构件为顶，装饰构件点睛——弟子明白。",
    "[老者]正是。每合成一种构件，我们便能推进工程，我也将为你讲解每座建筑的规制与意义。",
    "[青年]那弟子该从何做起？",
    "[老者]先去工地寻找材料吧。集齐基础材料，在作坊中合成。记住，材料各有用途，合成需谨慎。",
    "[青年]弟子谨记！定不负师父所托。",
    "[老者]去吧，让我们一砖一瓦，筑起大明荣耀。"
            }
        },
        { "stone_component", new string[]
    {
        "[老者]此乃紫禁城之基石。奉天门、奉天殿、华盖殿、谨身殿，四座巨构，无一不依赖坚固的石基。",
        "[青年]师父，弟子明白。台基需以青白石层层叠砌，既承重荷，又显庄严。",
        "[老者]正是。石构件关乎建筑之寿命，每一块石料皆需精挑细刻，毫厘不差。",
        "[青年]弟子定当严加勘察，确保基础万无一失。",
        "[老者]好！根基稳固，方能托起大明千秋功业。"
    }
},

{ "wood_component", new string[]
    {
        "[老者]木构乃建筑之骨架。奉天殿面阔九间，进深五间，所用梁柱皆需巨木，榫卯必须严丝合缝。",
        "[青年]师父，楠木坚韧，松木轻巧，如何选用方为得当？",
        "[老者]依形制而定。承重处用楠木，辅以松木；榫卯结构务必精巧，以柔克刚，方能抗震千年。",
        "[青年]弟子记住了。木料干燥、防腐亦不可疏忽。",
        "[老者]甚好。木工之道，在于心手合一，方能造出传世之器。"
    }
},

{ "tile_component", new string[]
    {
        "[老者]琉璃瓦乃皇家气象之点睛。奉天殿覆重檐庑殿顶，华盖殿为圆攒尖顶，瓦色形制皆有规制。",
        "[青年]黄色琉璃瓦为帝室专用，色泽需纯正，釉面需光润。",
        "[老者]不错。瓦片排列须疏密得当，既防雨雪，又显威严。每一片皆需火候精准，方成上品。",
        "[青年]弟子会亲自监督烧制，确保片片精良。",
        "[老者]紫禁城之顶，亦是大明之顶，容不得半点马虎。"
    }
},

{ "decoration_component", new string[]
    {
        "[老者]雕梁画栋，龙纹凤饰，乃皇权之象征。奉天门、谨身殿的彩画与雕刻，需彰显天子威仪。",
        "[青年]师父，龙纹有五爪，云纹有翻卷，弟子已熟记其法。",
        "[老者]心到，手到，神到。每一刀每一笔，都须倾注心血，方能让飞龙栩栩如生。",
        "[青年]弟子必以敬畏之心，精雕细琢，不负圣恩。",
        "[老者]好！愿这紫禁城因我们的匠心，而成为后世仰望的传奇。"
    }
}
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