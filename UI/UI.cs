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

    //记录界面的开关情况
    bool isOpen = false;
    GameObject currentUI;

    private void Awake()
    {
        //switchTo(skillTreeUI);
        fadeScreen.gameObject.SetActive(true);    
        
    }

    void Start()
    {
        switchTo(null);     //游戏开始时，不显示菜单UI
        switchTo(inGameUI); //游戏开始时，直接打开左上角角色血条UI

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    void Update()
    {
        isOpen = characterUI.activeSelf;

        //控制Character界面UI的开关
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

        //控制Option界面UI的开关
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
        //控制Craft界面UI的开关
        //if (Input.GetKeyDown(KeyCode.B))    switchTo(craftUI);

        //控制Skill Tree界面UI的开关
        //if (Input.GetKeyDown(KeyCode.K))    switchTo(skillTreeUI);


        //使用道具栏的道具
        UpdateUseStash();

        //问题：未知原因，在开启游戏时，Stash栏自动设为FALSE
        StashUI.SetActive(true);
    }

    //控制UI界面的开启
    public void switchTo(GameObject _menu)
    {

        for(int i = 0; i < transform.childCount; i++)
        {
            //检测画面渐进是否开启
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;

            //如果没开启，则：
            if(!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if(_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    //控制UI界面的关闭
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

    //作用：防止在Character栏关闭后，InGameUI无法打开
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
        yield return new WaitForSeconds(1); //等待1秒钟时间
        endText.SetActive(true);            //显示文字
        yield return new WaitForSeconds(1);
        restartButton.SetActive(true);      //显示按钮
    }

    public void restartGameButton() => GameManager.instance.restartScene();

    //放入Update()中，不断判断是否使用道具栏
    public void UpdateUseStash()
    {
        List<InventoryItem> stash = Inventory.instance.stash; //获取道具栏中物品信息

        if (Input.GetKeyDown(KeyCode.Alpha1) && stash[0].data)
        {
            ItemData item = stash[0].data;
            if(item.itemType == ItemType.UsableItem) item.UseItem();
            Debug.Log("使用道具栏1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && stash[1].data)
        {
            ItemData item = stash[1].data;
            if (item.itemType == ItemType.UsableItem) item.UseItem();
            Debug.Log("使用道具栏2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && stash[2].data)
        {
            ItemData item = stash[2].data;
            if (item.itemType == ItemType.UsableItem) item.UseItem();
            Debug.Log("使用道具栏3");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && stash[3].data)
        {
            ItemData item = stash[3].data;
            if (item.itemType == ItemType.UsableItem) item.UseItem();
            Debug.Log("使用道具栏4");
        }
    }
}
