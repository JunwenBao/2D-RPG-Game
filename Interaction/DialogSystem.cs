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
    [SerializeField] private TextMeshProUGUI sentence_text; //�Ի��ı�
    [SerializeField] private TextMeshProUGUI name_text;     //�����ı�
    //public Image faceImage; //ͷ��

    [Header("TEXT")]
    public TextAsset textFile;    //�ı��ļ�
    public int index;
    public float textSpeed = .08f; //�ı�����ٶ�
    bool textFinished;            //�ж��ı��Ƿ��ӡ����
    bool cancelTyping;            //�ж��Ƿ������ʾ�ı�
    List<string> textList = new List<string>();

    //[Header("Head Image")]
    //public Sprite face01, face02;

    // Start is called before the first frame update
    void Awake()
    {
        index = 0;
        getTextFromFile(textFile);
    }

    //���Ի����Activeʱ��ֱ����ʾ�ı��еĵ�һ�����֣�ע�⣺OnEnable����Start֮ǰ����
    private void OnEnable()
    {
        textFinished = true;
        StartCoroutine(setTextUI());
    }

    // Update is called once per frame
    void Update()
    {
        //����Ի���������ȡ�ı�����󣩣���ֱ�ӹر�
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

    //��ȡ�ı��ļ�(.txt)�е��ı������зָ������뵽textList��
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

        judgeName();

        //ִ�д���Ч��
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
