using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";    //存储文件路径
    private string dataFileName = "";   //存储文件名

    private bool encryptData = false;
    private string codeWord = "bao";

    public FileDataHandler(string _dataDirPath, string _dataFileName, bool _encryptData)
    {
        this.dataDirPath = _dataDirPath;
        this.dataFileName = _dataFileName;
        this.encryptData = _encryptData;
    }

    //保存
    public void save(GameDataScriptable _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(_data, true);

            //如果需要数据加密，则执行加密函数
            if(encryptData) dataToStore = encryptDecrypt(dataToStore);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error on tring to save data to file: " + fullPath + "\n" + e);
        }
    }

    //加载
    public GameDataScriptable load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameDataScriptable loadData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using(FileStream stream = new FileStream(fullPath, FileMode .Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //如果数据被加密：则执行解密
                if(encryptData) dataToLoad = encryptDecrypt(dataToLoad);

                loadData = JsonUtility.FromJson<GameDataScriptable>(dataToLoad);
            }

            catch (Exception e)
            {
                Debug.LogError("Error on try to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadData;
    }

    public void delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if(File.Exists(fullPath)) File.Delete(fullPath);
    }

    private string encryptDecrypt(string _data)
    {
        string modifiedData = "";

        for(int i = 0; i < _data.Length; i++)
        {
            modifiedData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);
        }

        return modifiedData;
    }
}
