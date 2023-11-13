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

    public Image itemImage;  // ������ �̹���
    [SerializeField]
    private int itemPrice; //ȹ���� �������� ����
    [SerializeField]
    private TextMeshProUGUI text_Count;
    [SerializeField]
    private TextMeshProUGUI text_Name; //�̸�
    [SerializeField]
    public GameObject itemObject; //���Կ� ���� ������ ������Ʈ
    [SerializeField]
    type SlotType;

    public bool spriteChangeIgnore; //��ų�����̳� ���⽽���� ��� �⺻��������Ʈ�� �ٸ��⶧���� üũ�ؼ�����
    bool isGrab = false;
   public GameObject GrabableObject;

    [SerializeField] bool typeCheck; //���� ���� Ÿ���� üũ

    private void Update()//������ ������ ���Ծȿ��ִ� ������Ʈ�� �ѱ�
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

    private void OnDisable() //������ ������ ���Ծȿ��ִ� ������Ʈ�� ����
    {
        if (itemObject != null && itemObject.GetComponent<Item>().SlotIn)
            itemObject.SetActive(false);
    }

    public void AddItem(GameObject obj) //�κ� �������߰�. item ��ũ��Ʈ�� InventoryPlay �޼��忡�� ���˴ϴ�.
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

        if (this != UIManager.instance.equipMent.weaponSlot) //���⽽�Կ� �ڵ������ɶ� ���Ⱑ ��������
            itemObject.SetActive(false);
    }

    public void ClearSlot() //���� �ʱ�ȭ
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
                { //���� ���ٽ����̴�
                    if (GrabableObject != null && GrabableObject.GetComponent<Armor>() &&
                        GrabableObject.GetComponent<Armor>().Type == UIManager.instance.Chest) //������ Ÿ���� ������ ������Ʈ��
                    {
                        typeCheck = true;
                    }
                }
                else if (this == UIManager.instance.equipMent.BootsSlot)
                {
                    //���� ���������̴�
                    if (GrabableObject != null && GrabableObject.GetComponent<Armor>() &&
                        GrabableObject.GetComponent<Armor>().Type == UIManager.instance.Boots)
                    {
                        typeCheck = true;
                    }

                }
                else if (this == UIManager.instance.equipMent.HeadSlot)
                {
                    //���� ��彽���̴�
                    if (GrabableObject != null && GrabableObject.GetComponent<Armor>() &&
                        GrabableObject.GetComponent<Armor>().Type == UIManager.instance.Head)
                    {
                        typeCheck = true;
                    }
                }
                else if (GrabableObject != null && this != UIManager.instance.equipMent.ArmorSlot
                    && this != UIManager.instance.equipMent.BootsSlot && this != UIManager.instance.equipMent.HeadSlot
                    && this != UIManager.instance.equipMent.weaponSlot)     //���� �κ��̶��
                {
                    typeCheck = true;
                }
                else //�κ����ƴϰ� ���â�ε�, ���� ������Ʈ�� Ÿ�԰� ���â�� Ÿ���� ���� �ٸ����
                {
                    typeCheck = false;
                }

                if (typeCheck)
                {
                    GrabableObject.GetComponent<Item>().ScaleSizeDown();
                    UIInteractor.instance.GrabControllerBoolCheck(true);
                    if (GrabableObject != null && GrabableObject.GetComponent<Item>() != null)
                        GrabableObject.GetComponent<Item>().slot = this;
                    if (!spriteChangeIgnore && isGrab) //�տ� ������ �������� ���򺯰�
                        itemImage.sprite = UIManager.instance.slotContactSprite;
                }
            }
            else
            {
                if (!isGrab)    //���� �׷��Ѿ������������� �����ۻ�������
                {
                    UIInteractor.instance.GrabControllerBoolCheck(false);
                    itemObject.SetActive(true);
                    itemObject.GetComponent<Item>().slot = this;
                    //itemObject.GetComponent<Item>().scale.DOMove(UIInteractor.instance.
                    //    GrabDirectInteractor.transform.position, 0.1f);
                    itemObject.GetComponent<Item>().scale.position = UIInteractor.instance.GrabDirectInteractor.transform.position;
                    itemObject.transform.position = itemObject.GetComponent<Item>().scale.position;
                    UIManager.instance.toolTip.GetComponent<ToolTip>().ToolTipOn(gameObject); //�����ڽ��� ������. �������� �����ϱ�����
                }
                else       //�׷���Ʈ�ѷ��� ������ ��������
                {
                    UIManager.instance.toolTip.GetComponent<ToolTip>().ToolTipOn(gameObject); //�����ڽ��� ������. �������� �����ϱ�����
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

        if (other.CompareTag("DirectController")) //�׷���Ʈ�ѷ��� ������ ��������
        {
            UIManager.instance.toolTip.GetComponent<ToolTip>().ToolTipOff(); //�����ڽ��� ������. �������� �����ϱ�����
            UIInteractor.instance.GrabControllerBoolCheck(false);

            if (itemObject != null && this != UIManager.instance.equipMent.weaponSlot) //���⽽���� ���ۻ�����°� ����
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