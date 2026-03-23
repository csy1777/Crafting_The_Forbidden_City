using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI 引用")]
    public GameObject dialoguePanel;   
    public Text dialogueText;          

    [Header("头像设置")]
    public Image leftAvatar;   
    public Image rightAvatar;  
    public Sprite elderSprite; 
    public Sprite youthSprite; 

    [Header("打字速度")]
    public float typingSpeed = 0.05f;  // 每个字符间隔秒数

    private string[] currentLines;     // 当前对话的所有句子
    private int currentLineIndex;      // 当前显示到第几句
    private bool isTyping;             // 是否正在打字
    private Coroutine typingCoroutine;

    void Awake()
    {
        
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

   
    private void ShowCurrentLine()
    {
        if (currentLineIndex < currentLines.Length)
        {
            string rawText = currentLines[currentLineIndex];
            string speaker;
            string displayText = ParseSpeaker(rawText, out speaker);

            // 根据说话者设置头像
            if (speaker == "elder")
            {
                leftAvatar.sprite = elderSprite;
                leftAvatar.gameObject.SetActive(true);
                rightAvatar.gameObject.SetActive(false);
            }
            else if (speaker == "youth")
            {
                rightAvatar.sprite = youthSprite;
                rightAvatar.gameObject.SetActive(true);
                leftAvatar.gameObject.SetActive(false);
            }
            else
            {
                // 没有标识符，隐藏所有头像
                leftAvatar.gameObject.SetActive(false);
                rightAvatar.gameObject.SetActive(false);
            }

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(displayText));
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

   
    private void HandleNextLine()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            string rawText = currentLines[currentLineIndex];
            string speaker;
            string displayText = ParseSpeaker(rawText, out speaker);
            dialogueText.text = displayText;
            isTyping = false;
            typingCoroutine = null;
        }
        else
        {
            currentLineIndex++;
            ShowCurrentLine();
        }
    }



    // 解析说话者标识，返回纯文本
    private string ParseSpeaker(string rawText, out string speaker)
    {
        speaker = "";
        if (rawText.StartsWith("[老者]"))
        {
            speaker = "elder";
            return rawText.Substring(4); // 去掉 "[老者]" 
        }
        else if (rawText.StartsWith("[青年]"))
        {
            speaker = "youth";
            return rawText.Substring(4);
        }
        return rawText;
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