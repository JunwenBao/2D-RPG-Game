using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//¿þÀÜ

public class UI_Shop_Slot : MonoBehaviour
{
    public static UI_Shop_Slot instance;

    public InventoryItem item;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
}
