using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CutsceneManager : MonoBehaviour
{
    public List<GameObject> imageList = new List<GameObject>();
    public int imageIndex;
    private GameObject currentImage;

    public TextAsset textFile;            //文本文件
    public TextMeshProUGUI sentence_text; //文本
    public List<string> textList = new List<string>();

    public float textSpeed = .1f; //文本输出速度
    bool textFinished;             //判断文本是否打印结束
    bool cancelTyping;             //判断是否快速显示文本
    public int textIndex;

    public string nextSceneName;

    private void Awake()
    {
        textIndex = 0;
        getTextFromFile(textFile);
    }

    //当对话面板Active时，直接显示文本中的第一行文字，注意：OnEnable会在Start之前调用
    private void OnEnable()
    {
        getTextFromFile(textFile);

        textFinished = true;
        StartCoroutine(setTextUI());

        imageList[imageIndex].SetActive(true); //显示第一张图片
    }

    private void Update()
    {
        //如果对话结束（读取文本到最后），则直接关闭
        if (Input.GetKeyUp(KeyCode.E) && textIndex == textList.Count)
        {
            gameObject.SetActive(false);
            return;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (textFinished && !cancelTyping)
            {
                StartCoroutine(setTextUI());

                //切换漫画图片
                if(imageIndex < imageList.Count - 1)
                {
                    imageList[imageIndex++].SetActive(false);
                    imageList[imageIndex].SetActive(true);
                }
            }
            else if (!textFinished)
            {
                cancelTyping = !cancelTyping;
            }
        }

        //如果到了最后一句，则跳到下一个指定的场景
        if(textIndex == textList.Count) SceneManager.LoadScene(nextSceneName);
    }

    //作用：获取文本文件(.txt)中的文本，按行分割，将其加入到textList中
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

        //执行打字效果
        int letter = 0;
        while (!cancelTyping && letter < textList[textIndex].Length - 1)
        {
            sentence_text.text += textList[textIndex][letter];
            letter++;

            yield return new WaitForSeconds(textSpeed);
        }

        sentence_text.text = textList[textIndex];
        cancelTyping = false;
        textFinished = true;
        textIndex++;
    }
}
