using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Board : NPCs
{
    public GameObject board;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (eButton.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            board.SetActive(true);
        }
    }
}
