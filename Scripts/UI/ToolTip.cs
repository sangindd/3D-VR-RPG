using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class ToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName; //아이템 이름
    [SerializeField] private TextMeshProUGUI itemGrade; //아이템 등급
    [SerializeField] private TextMeshProUGUI itemType; //아이템 타입
    [SerializeField] private TextMeshProUGUI dmg_or_defValue; //데미지 또는 방어력텍스트

    [SerializeField] private TextMeshProUGUI strText; //힘
    [SerializeField] private TextMeshProUGUI dexText; //민첩
    [SerializeField] private TextMeshProUGUI intText; //지능

    [SerializeField] private TextMeshProUGUI infoText; //설명

    [SerializeField] private Image itemImage; //아이템 이미지

    [SerializeField] private Vector3 originPos; //맨처음에 존재하던 위치. 왼손

    //슬롯에 들어가있는 아이템의 offset조정용
    public float yOffset = 0.14f;

    private float slotScale = 0.55f; //손에 달렸을때와 슬롯에 달렸을때 크기가 달라야 보기편함
    private float playerScale = 0.006f;

    public bool isShopToolTip; //내가 상점툴팁인가? 상점툴팁만 체크


    [SerializeField] private Transform player;
    [SerializeField] private GameObject itemObject; //건너온 아이템

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
    //        if (itemObject.GetComponent<Slot>())//툴팁을 표시해야할곳이 슬롯이다
    //        {
    //            transform.SetParent(null);
    //            transform.position = new Vector3(itemObject.transform.position.x, itemObject.transform.position.y + yOffset,
    //                  itemObject.transform.position.z);
    //            transform.rotation = itemObject.transform.rotation;
    //            transform.localScale = new Vector3(slotScale, slotScale, slotScale);
    //        }
    //        else if (itemObject.GetComponent<Item>() && !itemObject.GetComponent<Item>().SlotIn)
    //        {   //툴팁을 표시해야할곳이 밖에있는 오브젝트다
    //            transform.SetParent(UIInteractor.instance.RightHandToolTipPos);
    //            transform.localPosition = originPos;
    //            transform.localScale = new Vector3(playerScale, playerScale, playerScale);

    //        }
    //    }

    //}

    public void ToolTipOn(GameObject obj)
    {
        gameObject.SetActive(true);
        
        //건너온 오브젝트가 슬롯 밖에있는 아이템이다.
        if (obj.GetComponent<Item>() && !obj.GetComponent<Item>().SlotIn)
        {
            itemObject = obj;
            ItemExplanationSet(obj);
            transform.SetParent(UIInteractor.instance.RightHandToolTipPos);
            transform.localPosition = originPos;
            transform.localScale = new Vector3(playerScale, playerScale, playerScale);
        }
        //건너온 오브젝트가 슬롯이다.
        else if (obj.GetComponent<Slot>())
        {
            itemObject = obj;
            ItemExplanationSet(obj.GetComponent<Slot>().itemObject);
            if (!isShopToolTip) //상점 툴팁이 아닐경우만 포지션스케일변경, 상점툴팁은 위치고정
            {
                transform.SetParent(null);
                transform.position = new Vector3(itemObject.transform.position.x, itemObject.transform.position.y + yOffset,
                      itemObject.transform.position.z);
                transform.rotation = itemObject.transform.rotation;
                transform.localScale = new Vector3(slotScale, slotScale, slotScale);
            }
        }


        //밖에 있는 아이템의 경우 로테이션값을 처음만 한번 조정해준다. 업데이트에 집어넣으면 좀 어색하다
        if (itemObject != null && itemObject.GetComponent<Item>() && !itemObject.GetComponent<Item>().SlotIn)
            transform.rotation = player.rotation;
    }
    public void ToolTipOff()
    {
        itemObject = null;
        if (gameObject.activeSelf)
            gameObject.SetActive(false); //끌때 애니메이션으로 하니까 좀이상해서 뻈습니다
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
            strText.text = "힘 " + " +" + weapon.Strength.ToString();
            dexText.text = "민첩 " + " +" + weapon.Dexterity.ToString();
            intText.text = "지능 " + " +" + weapon.Intelligence.ToString();
            dmg_or_defValue.text = "<size=0.3>" + weapon.Damage_Basic.ToString() + "</size> 공격력";
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
            strText.text = "힘 " + " +" + armor.Strength.ToString();
            dexText.text = "민첩 " + " +" + armor.Dexterity.ToString();
            intText.text = "지능 " + " +" + armor.Intelligence.ToString();
            dmg_or_defValue.text = "<size=0.3>" + armor.Defense.ToString() + "</size> 방어력";
            infoText.text = armor.Info;
        }
    }
}