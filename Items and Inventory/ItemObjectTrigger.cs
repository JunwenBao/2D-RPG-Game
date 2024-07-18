using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject myItemObject =>GetComponentInParent<ItemObject>();

    //��ײ��������������Ʒ��ײ�����ʱ������ʰȡ��ע�⣺˫�����붼��Collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //����Ʒ�Ӵ�����Һ󣺼�����Ʒ
        if (collision.GetComponent<Player>() != null)
        {

            //�жϣ����������ˣ���ô�Ͳ��ᴥ��ʰȡ�����������������ʱ����Ʒ���书�ܣ������ʧȥ��Ʒ�������ٴ�ʰȡ��
            if (collision.GetComponent<CharacterStats>().isDead) return;

            Debug.Log("pick up items");
            myItemObject.pickupItem();
        }
    }
}
