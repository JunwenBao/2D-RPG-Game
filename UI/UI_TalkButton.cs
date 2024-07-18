using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TalkButton : MonoBehaviour
{
    public GameObject button;
    public GameObject talkUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        button.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        button.SetActive(false);
    }

    void Update()
    {
        if(button.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            talkUI.SetActive(true);
        }
    }
}
