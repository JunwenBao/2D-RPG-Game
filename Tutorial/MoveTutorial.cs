using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTutorial : MonoBehaviour
{
    public bool tutorial1 = false;
    public GameObject t1_GameObject;

    public bool tutorial2 = true;
    public GameObject t2_GameObject;

    private void Awake()
    {
        if(MoveTutorialManager.instance.isShow)
        {
            tutorial1 = true;
        }
    }

    private void Update()
    {
        if (!tutorial1 && (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)))
        {
            tutorial1 = true;
            t1_GameObject.SetActive(false);
            t2_GameObject.SetActive(true);
            tutorial2 = false;
        }

        if (!tutorial2 && Input.GetKeyUp(KeyCode.Space))
        {
            tutorial2 = true;
            t2_GameObject.SetActive(false);
        }
    }
}
