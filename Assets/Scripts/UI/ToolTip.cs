using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class ToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName; //������ �̸�
    [SerializeField] private TextMeshProUGUI itemGrade; //������ ���
    [SerializeField] private TextMeshProUGUI itemType; //������ Ÿ��
    [SerializeField] private TextMeshProUGUI dmg_or_defValue; //������ �Ǵ� �����ؽ�Ʈ

    [SerializeField] private TextMeshProUGUI strText; //��
    [SerializeField] private TextMeshProUGUI dexText; //��ø
    [SerializeField] private TextMeshProUGUI intText; //����

    [SerializeField] private TextMeshProUGUI infoText; //����

    [SerializeField] private Image itemImage; //������ �̹���

    [SerializeField] private Vector3 originPos; //��ó���� �����ϴ� ��ġ. �޼�

    //���Կ� ���ִ� �������� offset������
    public float yOffset = 0.14f;

    private float slotScale = 0.55f; //�տ� �޷������� ���Կ� �޷����� ũ�Ⱑ �޶�� ��������
    private float playerScale = 0.006f;

    public bool isShopToolTip; //���� ���������ΰ�? ���������� üũ


    [SerializeField] private Transform player;
    [SerializeField] private GameObject itemObject; //�ǳʿ� ������

    private void Start()
    {
        originPos = transform.localPosition;
    }

    public GameObject GetItemObject()
    {
        return itemObject;
    }


    //private void Update()
    //{
    //    if (itemObject != null)
    //    {
    //        if (itemObject.GetComponent<Slot>())//������ ǥ���ؾ��Ұ��� �����̴�
    //        {
    //            transform.SetParent(null);
    //            transform.position = new Vector3(itemObject.transform.position.x, itemObject.transform.position.y + yOffset,
    //                  itemObject.transform.position.z);
    //            transform.rotation = itemObject.transform.rotation;
    //            transform.localScale = new Vector3(slotScale, slotScale, slotScale);
    //        }
    //        else if (itemObject.GetComponent<Item>() && !itemObject.GetComponent<Item>().SlotIn)
    //        {   //������ ǥ���ؾ��Ұ��� �ۿ��ִ� ������Ʈ��
    //            transform.SetParent(UIInteractor.instance.RightHandToolTipPos);
    //            transform.localPosition = originPos;
    //            transform.localScale = new Vector3(playerScale, playerScale, playerScale);

    //        }
    //    }

    //}

    public void ToolTipOn(GameObject obj)
    {
        gameObject.SetActive(true);
        
        //�ǳʿ� ������Ʈ�� ���� �ۿ��ִ� �������̴�.
        if (obj.GetComponent<Item>() && !obj.GetComponent<Item>().SlotIn)
        {
            itemObject = obj;
            ItemExplanationSet(obj);
            transform.SetParent(UIInteractor.instance.RightHandToolTipPos);
            transform.localPosition = originPos;
            transform.localScale = new Vector3(playerScale, playerScale, playerScale);
        }
        //�ǳʿ� ������Ʈ�� �����̴�.
        else if (obj.GetComponent<Slot>())
        {
            itemObject = obj;
            ItemExplanationSet(obj.GetComponent<Slot>().itemObject);
            if (!isShopToolTip) //���� ������ �ƴҰ�츸 �����ǽ����Ϻ���, ���������� ��ġ����
            {
                transform.SetParent(null);
                transform.position = new Vector3(itemObject.transform.position.x, itemObject.transform.position.y + yOffset,
                      itemObject.transform.position.z);
                transform.rotation = itemObject.transform.rotation;
                transform.localScale = new Vector3(slotScale, slotScale, slotScale);
            }
        }


        //�ۿ� �ִ� �������� ��� �����̼ǰ��� ó���� �ѹ� �������ش�. ������Ʈ�� ��������� �� ����ϴ�
        if (itemObject != null && itemObject.GetComponent<Item>() && !itemObject.GetComponent<Item>().SlotIn)
            transform.rotation = player.rotation;
    }
    public void ToolTipOff()
    {
        itemObject = null;
        if (gameObject.activeSelf)
            gameObject.SetActive(false); //���� �ִϸ��̼����� �ϴϱ� ���̻��ؼ� �Q���ϴ�
    }

    private void ItemExplanationSet(GameObject item)
    {
        if (item.GetComponent<Weapon>() != null)
        {
            var weapon = item.GetComponent<Weapon>();
            var color = UIManager.instance.ItemGradeColor(weapon.gameObject);
            itemImage.sprite = weapon.Sprite;
            itemName.text = weapon.ItemName;
            itemName.color = color;
            itemGrade.text = weapon.Rank;
            itemGrade.color = color;
            itemType.text = weapon.Type;
            itemType.color = color;
            strText.text = "�� " + " +" + weapon.Strength.ToString();
            dexText.text = "��ø " + " +" + weapon.Dexterity.ToString();
            intText.text = "���� " + " +" + weapon.Intelligence.ToString();
            dmg_or_defValue.text = "<size=0.3>" + weapon.Damage_Basic.ToString() + "</size> ���ݷ�";
            infoText.text = weapon.Info;
        }
        else if (item.GetComponent<Armor>() != null)
        {
            var armor = item.GetComponent<Armor>();
            var color = UIManager.instance.ItemGradeColor(armor.gameObject);
            itemImage.sprite = armor.Sprite;
            itemName.text = armor.ItemName;
            itemName.color = color;
            itemGrade.text = armor.Rank;
            itemGrade.color = color;
            itemType.text = armor.Type;
            itemType.color = color;
            strText.text = "�� " + " +" + armor.Strength.ToString();
            dexText.text = "��ø " + " +" + armor.Dexterity.ToString();
            intText.text = "���� " + " +" + armor.Intelligence.ToString();
            dmg_or_defValue.text = "<size=0.3>" + armor.Defense.ToString() + "</size> ����";
            infoText.text = armor.Info;
        }
    }
}