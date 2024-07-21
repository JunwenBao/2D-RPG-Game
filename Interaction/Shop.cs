using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    void Start()
    {
        
    }


    void Update()
    {
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            gameObject.SetActive(false);
    }

    public void buy()
    {

    }
}
