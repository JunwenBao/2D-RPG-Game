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

    [Header("TEXT")]
    public TextAsset textFile;     //文本文件
    public int index;              //文本中的行指针
    public int aIndex;   
    public int bIndex;
    public float textSpeed = .08f; //文本输出速度
    bool textFinished;             //判断文本是否打印结束
    bool cancelTyping;             //判断是否快速显示文本
    List<string> textList = new List<string>();

    [Header("Head Image")]
    public GameObject faceImage1;      //显示头像1
    public GameObject faceImage2;      //显示头像2
    public Sprite face01, face02; //存储人物头像，其中，face01为主角，face02则需要获取

    // Start is called before the first frame update
    void Awake()
    {
        textFile = DialogManager.instance.textFile;
        //face02 = DialogManager.instance.avatar; //获取NPC头像

        index = 0;
        aIndex = 2;
        bIndex = 1;

        getTextFromFile(textFile);
        Debug.Log("对话系统开");
    }

    //当对话面板Active时，直接显示文本中的第一行文字，注意：OnEnable会在Start之前调用
    private void OnEnable()
    {
        textFile = DialogManager.instance.textFile;
        getTextFromFile(textFile);

        faceImage1.GetComponent<Image>().sprite = face01;
        faceImage2.GetComponent<Image>().sprite = DialogManager.instance.avatar;

        textFinished = true;
        StartCoroutine(setTextUI());
    }

    // Update is called once per frame
    void Update()
    {
        textFile = DialogManager.instance.textFile;
        //faceImage1.GetComponent<Image>().sprite = face01;
        //faceImage2.GetComponent<Image>().sprite = face02;
        if (aIndex - index == 1)
        {
            aIndex += 2;
            bIndex++;
        }
        if (bIndex % 2 == 0)
        {
            faceImage1.SetActive(true);
            faceImage2.SetActive(false);
        }
        else
        {
            faceImage1.SetActive(false);
            faceImage2.SetActive(true);
        }

        //如果对话结束（读取文本到最后），则直接关闭
        if (Input.GetKeyUp(KeyCode.E) && index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            aIndex = 2;
            bIndex = 1;
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
            
            //PlayerManager.instance.player.isTalking = true;
            //NPCManager.instance.NPC01.isTalking = true;
        }

        if (textList[index].ToString() == "Stranger")
        {
            //faceImage.sprite = face02;

            faceImage1.SetActive(true);
            faceImage2.SetActive(false);
            name_text.text = textList[index].ToString();
            index++;
            //PlayerManager.instance.player.isTalking = false;
            //NPCManager.instance.NPC01.isTalking = true;
        }
    }
}
