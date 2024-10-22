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

    public TextAsset textFile;            //�ı��ļ�
    public TextMeshProUGUI sentence_text; //�ı�
    public List<string> textList = new List<string>();

    public float textSpeed = .1f; //�ı�����ٶ�
    bool textFinished;             //�ж��ı��Ƿ��ӡ����
    bool cancelTyping;             //�ж��Ƿ������ʾ�ı�
    public int textIndex;

    public string nextSceneName;

    private void Awake()
    {
        textIndex = 0;
        getTextFromFile(textFile);
    }

    //���Ի����Activeʱ��ֱ����ʾ�ı��еĵ�һ�����֣�ע�⣺OnEnable����Start֮ǰ����
    private void OnEnable()
    {
        getTextFromFile(textFile);

        textFinished = true;
        StartCoroutine(setTextUI());

        imageList[imageIndex].SetActive(true); //��ʾ��һ��ͼƬ
    }

    private void Update()
    {
        //����Ի���������ȡ�ı�����󣩣���ֱ�ӹر�
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

                //�л�����ͼƬ
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

        //����������һ�䣬��������һ��ָ���ĳ���
        if(textIndex == textList.Count) SceneManager.LoadScene(nextSceneName);
    }

    //���ã���ȡ�ı��ļ�(.txt)�е��ı������зָ������뵽textList��
    private void getTextFromFile(TextAsset file)
    {
        textList.Clear();

        var lineData = file.text.Split('\n'); //���ı��������

        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    IEnumerator setTextUI()
    {
        textFinished = false;
        sentence_text.text = "";

        //ִ�д���Ч��
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
