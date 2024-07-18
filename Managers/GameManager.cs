using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    [SerializeField] private CheckPoint[] checkpoints; //�������еĴ浵��
    [SerializeField] private string closestCheckpointID;

    private void Start()
    {
        checkpoints = FindObjectsOfType<CheckPoint>();
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public void restartScene()
    { 
        SaveManager.instance.saveGame();

        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(scene.name);
    }

    public void loadData(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (CheckPoint checkpoint in checkpoints)
            {
                if (checkpoint.checkpointID == pair.Key && pair.Value == true)
                {
                    checkpoint.activeCheckpoint();
                }
            }
        }

        closestCheckpointID = _data.closestCheckpointID;

        Invoke("placePlayerAtClosestCheckpoint", .1f);
    }

    //?Why Extract??????
    private void placePlayerAtClosestCheckpoint()
    {
        foreach (CheckPoint checkpoint in checkpoints)
        {
            //����浵ʱ������ɫ��λ�ö�λ�����Ĵ浵��
            if (closestCheckpointID == checkpoint.checkpointID)
            {
                Vector2 bornPos = checkpoint.transform.position;
                bornPos.y -= 1.9f;
                PlayerManager.instance.player.transform.position = bornPos;
            }

        }
    }

    public void saveData(ref GameData _data)
    {
        _data.closestCheckpointID = findClosestCheckpoint().checkpointID;
        _data.checkpoints.Clear();

        foreach(CheckPoint checkPoint in checkpoints)
        {
            _data.checkpoints.Add(checkPoint.checkpointID, checkPoint.activationStatus);
        }
    }

    //Ѱ�Ҿ����ɫ����Ĵ浵��
    private CheckPoint findClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        CheckPoint closestCheckpoint = null;

        foreach(var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.instance.player.transform.position, checkpoint.transform.position);

            //�ҳ���������Ĵ浵�㣬�����棬֮�󷵻�
            if(distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }
}
