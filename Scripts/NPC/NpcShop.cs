using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcShop : Npc
{
    //�������� �� ������ �������
    [SerializeField] GameObject[] ItemPrefabs;

    //������ ������ ��������� ���� (�ܺ� ������)
    [SerializeField]
    private GameObject[] items;
    public GameObject[] Items { get { return items; } }

    [SerializeField]
    private GameObject ShopUI; //shopUI

    public Transform itemSpawnPoint; //�����ϸ� �������� ���ð� ���� ���̺�

    [SerializeField] ShopSlots shopSlots;
    [SerializeField] ToolTip tooltip;

    private void Awake()
    {
        UIEventManager.OnShopNPCInteraction += UIEventManager_OnShopNPCInteraction;
    }

    private void UIEventManager_OnShopNPCInteraction(Npc npc) //UI���ͷ��Ϳ��� ���ǽø� ���� Ʈ���Ź�ư ������ ����
    {
        if (npc.Name_ == Name_)   //�Ѿ�� ���ǽ��� �̸��� ���̸��� ���ٸ� 
            ShopOpen();
    }

    private void Start()
    {
        shopSlots = GetComponentInChildren<ShopSlots>(true);
        items = new GameObject[ItemPrefabs.Length]; // �迭 �ʱ�ȭ;

        for (int i = 0; i < ItemPrefabs.Length; i++) //ó�� �����ϸ� �ڽ����Ǹ��ϴ� ������ ���λ���
        {
            items[i] = Instantiate(ItemPrefabs[i], itemSpawnPoint.position, itemSpawnPoint.rotation);
            Items[i].name = ItemPrefabs[i].name; //�����Ǹ� �̸��ڿ� (clone)�� �پ DB�� ���ҷ��´�
        }
        StartCoroutine(ItemInit());
    }

    public void ShopOpen() //���ǽ�Ŭ���� ��������
    {
        if (!ShopUI.activeSelf)
            ShopUI.SetActive(true);

        shopSlots.ItemSetting(items);
    }

    IEnumerator ItemInit()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < items.Length; i++) //�����ϰ� DB �����͹ް� �����ϱ⶧���� �ð����ʿ��ϴ�
        {
            items[i].SetActive(false);
        }
    }

    public void Buy() //������ ���Ź�ư
    {
        //������ �����մ� �����۵����Ϳ� ������ ������ �ֱ⶧���� �����������ؼ� �� �ȿ��ִ� �������� ������
        tooltip.GetItemObject().GetComponent<Slot>().itemObject.gameObject.SetActive(true);
        tooltip.GetItemObject().GetComponent<Slot>().transform.parent.transform.parent.gameObject.SetActive(false);
        //�������� ��������
        tooltip.gameObject.SetActive(false); //������������
        
    }

    public void Cancle() //����â ��ҹ�ư   �̺�Ʈ�� Gameobject.Setactive�Ҵ��ϸ� ó���������� �ȸ����⶧����
    {
        tooltip.gameObject.SetActive(false);
    }
}