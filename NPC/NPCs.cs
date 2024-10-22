using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCs : MonoBehaviour
{
    public GameObject eButton;
    public GameObject talkUI;

    public Sprite avatar;         //ͷ��
    public TextAsset textFile;    //�ı��ļ�

    //����E��ť�ĳ�������ʧ
    public void OnTriggerEnter2D(Collider2D other)
    {
        eButton.SetActive(true);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        eButton.SetActive(false);
    }

    void Update()
    {
        if (eButton.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            DialogManager.instance.avatar = avatar;
            DialogManager.instance.textFile = textFile;
            talkUI.SetActive(true);
        }
    }
}
