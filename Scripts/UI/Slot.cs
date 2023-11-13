using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum type
    {
        Armor,
        Boots,
        Head,
        Inven,
        Shop
    }

    public Image itemImage;  // 슬롯의 이미지
    [SerializeField]
    private int itemPrice; //획득한 아이템의 가격
    [SerializeField]
    private TextMeshProUGUI text_Count;
    [SerializeField]
    private TextMeshProUGUI text_Name; //이름
    [SerializeField]
    public GameObject itemObject; //슬롯에 들어온 아이템 오브젝트
    [SerializeField]
    type SlotType;

    public bool spriteChangeIgnore; //스킬슬롯이나 무기슬롯의 경우 기본스프라이트가 다르기때문에 체크해서제외
    bool isGrab = false;
   public GameObject GrabableObject;

    [SerializeField] bool typeCheck; //닿은 방어구의 타입을 체크

    private void Update()//슬롯이 켜질때 슬롯안에있는 오브젝트도 켜기
    {
        //if (itemObject != null)
        //    if (text_Name.text != itemObject.name)
        //        try
        //        {
        //            AddItem(itemObject);
        //        }
        //        catch
        //        {

        //        };
    }

    private void OnDisable() //슬롯이 꺼질때 슬롯안에있는 오브젝트도 끄기
    {
        if (itemObject != null && itemObject.GetComponent<Item>().SlotIn)
            itemObject.SetActive(false);
    }

    public void AddItem(GameObject obj) //인벤 아이템추가. item 스크립트의 InventoryPlay 메서드에서 사용됩니다.
    {
        if (!obj.GetComponent<Item>())
            return;

        if (obj.GetComponent<Weapon>() is null)
        {
            Armor itemScript = obj.GetComponent<Armor>();
            //text_Name.text = itemScript.ItemName;
            itemImage.sprite = itemScript.Sprite;
            itemObject = itemScript.gameObject;
        }
        else
        {
            Weapon itemScript = obj.GetComponent<Weapon>();
            //text_Name.text = itemScript.ItemName;
            itemImage.sprite = itemScript.Sprite;
            itemObject = itemScript.gameObject;
        }

        if (this != UIManager.instance.equipMent.weaponSlot) //무기슬롯에 자동장착될때 무기가 꺼져버림
            itemObject.SetActive(false);
    }

    public void ClearSlot() //슬롯 초기화
    {
        itemObject = null;
        itemImage.sprite = UIManager.instance.slotDefaultSprite;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (UIInteractor.instance.GrabInteractableObject == null)
        {
            GrabableObject = null;
            isGrab = false;
        }
        else
        {
            GrabableObject = UIInteractor.instance.GrabInteractableObject;
            isGrab = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DirectController"))
        {
            if (itemObject == null)
            {
                if (this == UIManager.instance.equipMent.ArmorSlot)
                { //내가 갑바슬롯이다
                    if (GrabableObject != null && GrabableObject.GetComponent<Armor>() &&
                        GrabableObject.GetComponent<Armor>().Type == UIManager.instance.Chest) //닿은게 타입이 갑옷인 오브젝트면
                    {
                        typeCheck = true;
                    }
                }
                else if (this == UIManager.instance.equipMent.BootsSlot)
                {
                    //내가 부츠슬롯이다
                    if (GrabableObject != null && GrabableObject.GetComponent<Armor>() &&
                        GrabableObject.GetComponent<Armor>().Type == UIManager.instance.Boots)
                    {
                        typeCheck = true;
                    }

                }
                else if (this == UIManager.instance.equipMent.HeadSlot)
                {
                    //내가 헤드슬롯이다
                    if (GrabableObject != null && GrabableObject.GetComponent<Armor>() &&
                        GrabableObject.GetComponent<Armor>().Type == UIManager.instance.Head)
                    {
                        typeCheck = true;
                    }
                }
                else if (GrabableObject != null && this != UIManager.instance.equipMent.ArmorSlot
                    && this != UIManager.instance.equipMent.BootsSlot && this != UIManager.instance.equipMent.HeadSlot
                    && this != UIManager.instance.equipMent.weaponSlot)     //내가 인벤이라면
                {
                    typeCheck = true;
                }
                else //인벤도아니고 장비창인데, 닿은 오브젝트의 타입과 장비창의 타입이 서로 다를경우
                {
                    typeCheck = false;
                }

                if (typeCheck)
                {
                    GrabableObject.GetComponent<Item>().ScaleSizeDown();
                    UIInteractor.instance.GrabControllerBoolCheck(true);
                    if (GrabableObject != null && GrabableObject.GetComponent<Item>() != null)
                        GrabableObject.GetComponent<Item>().slot = this;
                    if (!spriteChangeIgnore && isGrab) //손에 뭔가가 있을때만 색깔변경
                        itemImage.sprite = UIManager.instance.slotContactSprite;
                }
            }
            else
            {
                if (!isGrab)    //현재 그랩한아이템이있을땐 아이템생성막기
                {
                    UIInteractor.instance.GrabControllerBoolCheck(false);
                    itemObject.SetActive(true);
                    itemObject.GetComponent<Item>().slot = this;
                    //itemObject.GetComponent<Item>().scale.DOMove(UIInteractor.instance.
                    //    GrabDirectInteractor.transform.position, 0.1f);
                    itemObject.GetComponent<Item>().scale.position = UIInteractor.instance.GrabDirectInteractor.transform.position;
                    itemObject.transform.position = itemObject.GetComponent<Item>().scale.position;
                    UIManager.instance.toolTip.GetComponent<ToolTip>().ToolTipOn(gameObject); //슬롯자신을 보낸다. 슬롯위에 생성하기위해
                }
                else       //그랩컨트롤러가 닿으면 툴팁생성
                {
                    UIManager.instance.toolTip.GetComponent<ToolTip>().ToolTipOn(gameObject); //슬롯자신을 보낸다. 슬롯위에 생성하기위해
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DirectController") && itemObject == null && !spriteChangeIgnore)
        {
            itemImage.sprite = UIManager.instance.slotDefaultSprite;
        }

        if (other.CompareTag("DirectController")) //그랩컨트롤러가 닿으면 툴팁제거
        {
            UIManager.instance.toolTip.GetComponent<ToolTip>().ToolTipOff(); //슬롯자신을 보낸다. 슬롯위에 생성하기위해
            UIInteractor.instance.GrabControllerBoolCheck(false);

            if (itemObject != null && this != UIManager.instance.equipMent.weaponSlot) //무기슬롯은 아템사라지는거 막기
                itemObject.SetActive(false);
            else if (GrabableObject != null)
                GrabableObject.GetComponent<Item>().ScaleSizeUp();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // if (item != null)
        // UIManager.instance.toolTip.ToolTipOn(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // UIManager.instance.toolTip.ToolTipOff();
    }

}