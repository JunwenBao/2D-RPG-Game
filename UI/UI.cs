using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [Header("End Screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject StashUI;

    public UI_Item_ToolTip itemToolTip;
    public UI_Stat_ToolTip statToolTip;

    //��¼����Ŀ������
    bool isOpen = false;
    GameObject currentUI;

    private void Awake()
    {
        //switchTo(skillTreeUI);
        fadeScreen.gameObject.SetActive(true);    
        
    }

    void Start()
    {
        switchTo(null);     //��Ϸ��ʼʱ������ʾ�˵�UI
        switchTo(inGameUI); //��Ϸ��ʼʱ��ֱ�Ӵ����Ͻǽ�ɫѪ��UI

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    void Update()
    {
        isOpen = characterUI.activeSelf;

        //����Character����UI�Ŀ���
        if (Input.GetKeyDown(KeyCode.Tab) && !isOpen)
        {
            characterUI.SetActive(true);
            currentUI = characterUI;
        }
        if (Input.GetKeyDown(KeyCode.Tab) && isOpen)
        {
            characterUI.SetActive(false);
            currentUI = null;
        }

        //����Option����UI�Ŀ���
        if (Input.GetKeyDown(KeyCode.Escape) && isOpen) 
        {
            currentUI.SetActive(false);
            optionUI.SetActive(true);
            currentUI = optionUI;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isOpen)
        {
            optionUI.SetActive(true);
            currentUI = optionUI;
        }
        //����Craft����UI�Ŀ���
        //if (Input.GetKeyDown(KeyCode.B))    switchTo(craftUI);

        //����Skill Tree����UI�Ŀ���
        //if (Input.GetKeyDown(KeyCode.K))    switchTo(skillTreeUI);


        //ʹ�õ������ĵ���
        UpdateUseStash();

        //���⣺δ֪ԭ���ڿ�����Ϸʱ��Stash���Զ���ΪFALSE
        StashUI.SetActive(true);
    }

    //����UI����Ŀ���
    public void switchTo(GameObject _menu)
    {

        for(int i = 0; i < transform.childCount; i++)
        {
            //��⻭�潥���Ƿ���
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;

            //���û��������
            if(!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if(_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    //����UI����Ĺر�
    private void switchWithKeyTo(GameObject _menu)
    {
        if(_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);

            checkForGameUI();

            return;
        }

        switchTo(_menu);
    }

    //���ã���ֹ��Character���رպ�InGameUI�޷���
    private void checkForGameUI()
    {
        for(int i = 0; i < transform.childCount;i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf) return;
        }

        switchTo(inGameUI);
    }

    public void switchOnEndScreen()
    {
        //fadeScreen.fadeOut();
        StartCoroutine(endScreenCorutine());
    }

    IEnumerator endScreenCorutine()
    {
        yield return new WaitForSeconds(1); //�ȴ�1����ʱ��
        endText.SetActive(true);            //��ʾ����
        yield return new WaitForSeconds(1);
        restartButton.SetActive(true);      //��ʾ��ť
    }

    public void restartGameButton() => GameManager.instance.restartScene();

    //����Update()�У������ж��Ƿ�ʹ�õ�����
    public void UpdateUseStash()
    {
        List<InventoryItem> stash = Inventory.instance.stash; //��ȡ����������Ʒ��Ϣ

        if (Input.GetKeyDown(KeyCode.Alpha1) && stash[0].data)
        {
            ItemData item = stash[0].data;
            if(item.itemType == ItemType.UsableItem) item.UseItem();
            Debug.Log("ʹ�õ�����1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && stash[1].data)
        {
            ItemData item = stash[1].data;
            if (item.itemType == ItemType.UsableItem) item.UseItem();
            Debug.Log("ʹ�õ�����2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && stash[2].data)
        {
            ItemData item = stash[2].data;
            if (item.itemType == ItemType.UsableItem) item.UseItem();
            Debug.Log("ʹ�õ�����3");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && stash[3].data)
        {
            ItemData item = stash[3].data;
            if (item.itemType == ItemType.UsableItem) item.UseItem();
            Debug.Log("ʹ�õ�����4");
        }
    }
}
