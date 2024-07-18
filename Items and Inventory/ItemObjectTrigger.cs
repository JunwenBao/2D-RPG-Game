using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject myItemObject =>GetComponentInParent<ItemObject>();

    //碰撞器触发：当该物品碰撞到玩家时，触发拾取。注意：双方必须都有Collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //当物品接触到玩家后：捡起物品
        if (collision.GetComponent<Player>() != null)
        {

            //判断：如果玩家死了，那么就不会触发拾取东西。用于玩家死亡时的物品掉落功能，让玩家失去物品而不会再次拾取。
            if (collision.GetComponent<CharacterStats>().isDead) return;

            Debug.Log("pick up items");
            myItemObject.pickupItem();
        }
    }
}
