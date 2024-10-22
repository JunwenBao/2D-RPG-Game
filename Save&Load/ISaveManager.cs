using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager
{
    void loadData(GameDataScriptable _data);

    void saveData(ref GameDataScriptable _data);
}
