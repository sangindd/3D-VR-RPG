using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcShop : Npc
{
    //상점에서 팔 아이템 프리펩들
    [SerializeField] GameObject[] ItemPrefabs;

    //생성된 아이템 프리펩들이 들어갈곳 (외부 참조용)
    [SerializeField]
    private GameObject[] items;
    public GameObject[] Items { get { return items; } }

    [SerializeField]
    private GameObject ShopUI; //shopUI

    public Transform itemSpawnPoint; //구매하면 아이템이 나올곳 보통 테이블

    [SerializeField] ShopSlots shopSlots;
    [SerializeField] ToolTip tooltip;

    private void Awake()
    {
        UIEventManager.OnShopNPCInteraction += UIEventManager_OnShopNPCInteraction;
    }

    private void UIEventManager_OnShopNPCInteraction(Npc npc) //UI인터렉터에서 엔피시를 보고 트리거버튼 누르면 실행
    {
        if (npc.Name_ == Name_)   //넘어온 엔피시의 이름과 내이름이 같다면 
            ShopOpen();
    }

    private void Start()
    {
        shopSlots = GetComponentInChildren<ShopSlots>(true);
        items = new GameObject[ItemPrefabs.Length]; // 배열 초기화;

        for (int i = 0; i < ItemPrefabs.Length; i++) //처음 시작하면 자신이판매하는 아이템 전부생성
        {
            items[i] = Instantiate(ItemPrefabs[i], itemSpawnPoint.position, itemSpawnPoint.rotation);
            Items[i].name = ItemPrefabs[i].name; //생성되면 이름뒤에 (clone)이 붙어서 DB를 못불러온다
        }
        StartCoroutine(ItemInit());
    }

    public void ShopOpen() //엔피시클릭시 상점오픈
    {
        if (!ShopUI.activeSelf)
            ShopUI.SetActive(true);

        shopSlots.ItemSetting(items);
    }

    IEnumerator ItemInit()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < items.Length; i++) //시작하고 DB 데이터받고 꺼야하기때문에 시간이필요하다
        {
            items[i].SetActive(false);
        }
    }

    public void Buy() //아이템 구매버튼
    {
        //툴팁이 갖고잇는 아이템데이터엔 무조건 슬롯이 있기때문에 슬롯을참조해서 그 안에있는 아이템을 꺼내기
        tooltip.GetItemObject().GetComponent<Slot>().itemObject.gameObject.SetActive(true);
        tooltip.GetItemObject().GetComponent<Slot>().transform.parent.transform.parent.gameObject.SetActive(false);
        //상점슬롯 꺼버리기
        tooltip.gameObject.SetActive(false); //툴팁꺼버리기
        
    }

    public void Cancle() //툴팁창 취소버튼   이벤트로 Gameobject.Setactive할당하면 처음열렸을때 안먹히기때문에
    {
        tooltip.gameObject.SetActive(false);
    }
}