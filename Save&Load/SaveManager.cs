using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Globalization;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName; //�洢���ļ���
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

        saveManagers = findAllSaveManagers(); //Ѱ�����еĽӿ�

        loadGame();
    }

    //����Ϸ
    public void newGame()
    {
        gameData= new GameData();
    }

    //������Ϸ�浵
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

    //������Ϸ
    public void saveGame()
    {
        //�������еĴ浵
        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.saveData(ref gameData);
        }

        dataHandler.save(gameData);
    }

    //��Ϸ�˳�������
    private void OnApplicationQuit()
    {
        saveGame();
    }

    //Ѱ�����еĽӿ�
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
