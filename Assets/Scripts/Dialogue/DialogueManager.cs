using System.Collections;
using UnityEngine;
using UnityEngine.UI;   // 如果用 TextMeshPro，请替换为 using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI 引用")]
    public GameObject dialoguePanel;   // 对话框面板
    public Text dialogueText;          // 显示文本的组件（若用 TMP 则类型为 TMP_Text）

    [Header("打字速度")]
    public float typingSpeed = 0.05f;  // 每个字符间隔秒数

    private string[] currentLines;     // 当前对话的所有句子
    private int currentLineIndex;      // 当前显示到第几句
    private bool isTyping;             // 是否正在打字
    private Coroutine typingCoroutine;

    void Awake()
    {
        // 单例模式，确保全局唯一
        if (Instance == null)
        {
            Instance = this;
        
        }
        else
        {
            Destroy(gameObject);
        }

        // 初始隐藏对话框
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void Update()
    {
        // 仅在对话框激活时监听按键（任意键）
        if (dialoguePanel.activeSelf && Input.anyKeyDown)
        {
            HandleNextLine();
        }
    }

    /// <summary>
    /// 通过字符串数组开始对话
    /// </summary>
    public void StartDialogue(string[] lines)
    {
        if (lines == null || lines.Length == 0) return;
        currentLines = lines;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        ShowCurrentLine();
    }

    // 显示当前句子（开启打字效果）
    private void ShowCurrentLine()
    {
        if (currentLineIndex < currentLines.Length)
        {
            string line = currentLines[currentLineIndex];
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(line));
        }
        else
        {
            EndDialogue();
        }
    }

    // 逐字打印协程
    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        typingCoroutine = null;
    }

    // 处理“下一句”逻辑
    private void HandleNextLine()
    {
        if (isTyping)
        {
            // 正在打字时：立即显示完整句子
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            dialogueText.text = currentLines[currentLineIndex];
            isTyping = false;
            typingCoroutine = null;
        }
        else
        {
            // 显示下一句
            currentLineIndex++;
            ShowCurrentLine();
        }
    }

    // 结束对话
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentLines = null;
        currentLineIndex = 0;
        isTyping = false;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
    }
}