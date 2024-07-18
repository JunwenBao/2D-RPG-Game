using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;

    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointID;

    //Constructor
    public GameData()
    {
        currency = 0;
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        closestCheckpointID = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();
    }
}
