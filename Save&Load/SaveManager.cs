using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Globalization;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName; //存储的文件名
    [SerializeField] private bool encryptData;

    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("Delete save file")]
    public void deleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.delete();
    }

    private void Awake()
    {
        if(instance != null) Destroy(instance.gameObject);
        else instance = this;
    }

    private void Start()
    {
        dataHandler  = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);

        saveManagers = findAllSaveManagers(); //寻找所有的接口

        loadGame();
    }

    //新游戏
    public void newGame()
    {
        gameData= new GameData();
    }

    //加载游戏存档
    public void loadGame()
    {
        gameData = dataHandler.load();

        if(this.gameData == null)
        {
            newGame();
        }

        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.loadData(gameData);
        }
    }

    //保存游戏
    public void saveGame()
    {
        //保存所有的存档
        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.saveData(ref gameData);
        }

        dataHandler.save(gameData);
    }

    //游戏退出：保存
    private void OnApplicationQuit()
    {
        saveGame();
    }

    //寻找所有的接口
    private List<ISaveManager> findAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }

    public bool hasNoSaveData()
    {
        if (dataHandler.load() != null) return true;

        return false;
    }
}
