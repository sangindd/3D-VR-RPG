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
        for (int i = 0; i < items.Length; i++)  //아이템 넣는 작업
        {
            slots[i].AddItem(items[i]); //이미지 세팅
            shopSlots[i].GetComponent<ShopSlotText>().TextSetting(items[i]); //텍스트세팅
        }

        for (int i = 0; i < slots.Length; i++)  //빈 슬롯이 아니면 활성화
        {
            if (slots[i].itemObject != null)
                shopSlots[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < slots.Length; i++) //빈 슬롯이면 비활성화
        {
            if (slots[i].itemObject == null)
                shopSlots[i].gameObject.SetActive(false);
        }

        scrollbar.value = 1f;
    }    
}