using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlots : MonoBehaviour
{
    public Slot[] slots;
    public GameObject[] shopSlots;

    public Scrollbar scrollbar;

    [SerializeField] ToolTip tooltip;


    void Awake()
    {
        slots = GetComponentsInChildren<Slot>(true);
    }

    public void ItemSetting(GameObject[] items)
    {
        for (int i = 0; i < items.Length; i++)  //������ �ִ� �۾�
        {
            slots[i].AddItem(items[i]); //�̹��� ����
            shopSlots[i].GetComponent<ShopSlotText>().TextSetting(items[i]); //�ؽ�Ʈ����
        }

        for (int i = 0; i < slots.Length; i++)  //�� ������ �ƴϸ� Ȱ��ȭ
        {
            if (slots[i].itemObject != null)
                shopSlots[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < slots.Length; i++) //�� �����̸� ��Ȱ��ȭ
        {
            if (slots[i].itemObject == null)
                shopSlots[i].gameObject.SetActive(false);
        }

        scrollbar.value = 1f;
    }    
}