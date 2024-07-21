using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI sentence_text; //对话文本
    [SerializeField] private TextMeshProUGUI name_text;     //名字文本
    //public Image faceImage; //头像

    [Header("TEXT")]
    public TextAsset textFile;    //文本文件
    public int index;
    public float textSpeed = .08f; //文本输出速度
    bool textFinished;            //判断文本是否打印结束
    bool cancelTyping;            //判断是否快速显示文本
    List<string> textList = new List<string>();

    //[Header("Head Image")]
    //public Sprite face01, face02;

    // Start is called before the first frame update
    void Awake()
    {
        index = 0;
        getTextFromFile(textFile);
    }

    //当对话面板Active时，直接显示文本中的第一行文字，注意：OnEnable会在Start之前调用
    private void OnEnable()
    {
        textFinished = true;
        StartCoroutine(setTextUI());
    }

    // Update is called once per frame
    void Update()
    {
        //如果对话结束（读取文本到最后），则直接关闭
        if(Input.GetKeyUp(KeyCode.E) && index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            PlayerManager.instance.player.isTalking = false;
            NPCManager.instance.NPC01.isTalking = false;
            return;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            if(textFinished && !cancelTyping)
            {
                StartCoroutine(setTextUI());
            }
            else if(!textFinished)
            {
                cancelTyping = !cancelTyping;
            }
        }
    }

    //获取文本文件(.txt)中的文本，按行分割，将其加入到textList中
    private void getTextFromFile(TextAsset file)
    {
        textList.Clear();

        var lineData = file.text.Split('\n'); //将文本按行输出

        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    IEnumerator setTextUI()
    {
        textFinished = false;
        sentence_text.text = "";

        judgeName();

        //执行打字效果
        int letter = 0;
        while (!cancelTyping && letter < textList[index].Length - 1)
        {
            sentence_text.text += textList[index][letter];
            letter++;

            yield return new WaitForSeconds(textSpeed);
        }

        sentence_text.text = textList[index];
        cancelTyping = false;
        textFinished = true;
        index++;
    }

    private void judgeName()
    {

        //???????????
        if (textList[index].ToString() != "Player")
        {
            Debug.Log("JUDGEGGGGGGG");
            //faceImage.sprite = face01;
            name_text.text = textList[index].ToString();
            index++;
            PlayerManager.instance.player.isTalking = true;
            NPCManager.instance.NPC01.isTalking = true;
        }

        if (textList[index].ToString() == "Stranger")
        {
            //faceImage.sprite = face02;
            name_text.text = textList[index].ToString();
            index++;
            PlayerManager.instance.player.isTalking = false;
            NPCManager.instance.NPC01.isTalking = true;
        }
    }
}
