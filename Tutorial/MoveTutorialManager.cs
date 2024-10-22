using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTutorialManager : MonoBehaviour
{
    public static MoveTutorialManager instance;

    public bool isShow;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        isShow = true;
    }
}
