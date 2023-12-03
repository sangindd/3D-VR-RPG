using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;
public class Item : MonoBehaviour
{
    public bool SlotIn; //���� ���Ծȿ��ִ»����ΰ�?
    public bool SlotStay; //���Կ� �����ִ»����ΰ�
    public bool QuickEquipmentSlotStay; //����񽽷Կ� �����ִ»����ΰ�?
    //public Transform ScaleObject; //ũ�������� ��ü (�����ֱ�)

    [SerializeField]
    private Vector3 originScale; //�⺻������
    private Vector3 originPosition;
    [SerializeField]
    private Vector3 smallScale; //�ٲ����� ������

    public Slot slot;
    public Transform scale;

    private void Awake()
    {
        originScale = scale.localScale;
        originPosition = scale.localPosition;
        smallScale = scale.localScale * 0.5f;
    }

    private void Update()
    {
        if (SlotIn)
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX
                | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        else
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EquipmentQuick") && !SlotIn && !GetComponent<Weapon>())
        {
            if (gameObject == UIInteractor.instance.LeftGrabInteractableObject)
            {
                UIInteractor.instance.LeftGrabDirectInteractor.SendHapticImpulse(0.25f, 0.1f);
            }
            else if (gameObject == UIInteractor.instance.GrabInteractableObject)
            {
                UIInteractor.instance.GrabDirectInteractor.SendHapticImpulse(0.25f, 0.1f);
            }
        }

        if (other.CompareTag("InventoryQuick"))
        {
            if (gameObject == UIInteractor.instance.LeftGrabInteractableObject)
            {
                UIInteractor.instance.LeftGrabDirectInteractor.SendHapticImpulse(0.25f, 0.1f);
            }
            else if (gameObject == UIInteractor.instance.GrabInteractableObject)
            {
                UIInteractor.instance.GrabDirectInteractor.SendHapticImpulse(0.25f, 0.1f);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        //---------------------���� ����-------------------------//
        if (other.CompareTag("GrabDirectController") &&  //��������
            UIInteractor.instance.GrabInteractableObject != gameObject &&//���� ����ִ� �������� �����ƴҶ���.
            !UIManager.instance.TooltipPanel.activeSelf && //������ ������������ ����.
             !SlotIn)
        {
            UIManager.instance.toolTip.ToolTipOn(gameObject);
        }
        //----------------------------------------------------------//

        //---------------------��� �κ�������-------------------------//
        if (other.CompareTag("InventoryQuick"))
        {
            var slots = UIManager.instance.inventorySlots;
            foreach (var slot in slots)
            {
                if (slot.itemObject == null)
                {
                    this.slot = slot;
                    SlotStay = true;
                    break;
                }
            }
            //slot.AddItem(UIInteractor.instance.GrabInteractableObject);
        }
        //----------------------------------------------------------//

        //---------------------��� ������-------------------------//
        if (other.CompareTag("EquipmentQuick") && !SlotIn && !GetComponent<Weapon>())
        {
            QuickEquipmentSlotStay = true;
        }
        //----------------------------------------------------------//
    }

    private void OnTriggerExit(Collider other)
    {
        InitSlot();

        if (other.CompareTag("GrabDirectController")) //���̷�Ʈ ��Ʈ�ѷ��� ����� ���� ����
        {
            UIManager.instance.toolTip.ToolTipOff();
        }

        //---------------------��� ������-------------------------//
        if (other.CompareTag("EquipmentQuick"))
        {
            QuickEquipmentSlotStay = false;
        }
        //----------------------------------------------------------//


        if (other.CompareTag("CharacterUI")) //�κ�â���� ��������� �׷���Ʈ�ѷ��� ���ڸ���
        {
            //UIInteractor.instance.GrabControllerBoolCheck(false);
        }
    }

    public void InventoryPlay() //�׷����ͷ��ͺ� �̺�Ʈ�ȿ�����
    {
        if (slot != null)
        {
            var ArmorSlot = UIManager.instance.equipMent.ArmorSlot;
            var BootsSlot = UIManager.instance.equipMent.BootsSlot;
            var HeadSlot = UIManager.instance.equipMent.HeadSlot;

            if (slot.GetComponent<Slot>().itemObject == null) //������ ������־���Ѵ�
            {
                if (slot == ArmorSlot && GetComponent<Armor>().Type ==
                    UIManager.instance.Chest) //���ٸ� �������
                {
                    if (gameObject == UIInteractor.instance.LeftGrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.LeftGrabInteractableObject);
                    }
                    else if (gameObject == UIInteractor.instance.GrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.GrabInteractableObject);
                    }

                    UIManager.instance.equipMent.OnArmorStat?.Invoke();
                }
                else if (slot == BootsSlot && GetComponent<Armor>().Type ==
                    UIManager.instance.Boots)//������
                {
                    if (gameObject == UIInteractor.instance.LeftGrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.LeftGrabInteractableObject);
                    }
                    else if (gameObject == UIInteractor.instance.GrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.GrabInteractableObject);
                    }
                    UIManager.instance.equipMent.OnBootsStat?.Invoke();
                }
                else if (slot == HeadSlot && GetComponent<Armor>().Type ==
                    UIManager.instance.Head)//����
                {
                    if (gameObject == UIInteractor.instance.LeftGrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.LeftGrabInteractableObject);
                    }
                    else if (gameObject == UIInteractor.instance.GrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.GrabInteractableObject);
                    }
                    UIManager.instance.equipMent.OnHeadStat?.Invoke();
                }
                else if (slot != ArmorSlot && slot != BootsSlot && slot != HeadSlot)//�κ�
                {
                    if (gameObject == UIInteractor.instance.LeftGrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.LeftGrabInteractableObject);
                    }
                    else if (gameObject == UIInteractor.instance.GrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.GrabInteractableObject);
                    }
                }
                SlotIn = true;
                ScaleSizeDown(); //������ �̵��ϸ� ũ�����̱�
                transform.rotation = Quaternion.identity;
            }

            else //���� �κ��ӿ� �ִٸ�
            {
                slot.ClearSlot();
                PositionInit();
                SlotIn = false;
                UIManager.instance.toolTip.ToolTipOff(); //�׷��ɶ� ������ �����ϱ�����
                //UIManager.instance.toolTip.GetComponent<ToolTip>().ToolTipOn(gameObject);
                if (slot == ArmorSlot && GetComponent<Armor>().Type ==
                    UIManager.instance.Chest) //���ٸ� �����������
                {
                    UIManager.instance.equipMent.OnArmorStat?.Invoke();
                }
                else if (slot == BootsSlot && GetComponent<Armor>().Type ==
                    UIManager.instance.Boots)
                {
                    UIManager.instance.equipMent.OnBootsStat?.Invoke();
                }
                else if (slot == HeadSlot && GetComponent<Armor>().Type ==
                    UIManager.instance.Head)
                {
                    UIManager.instance.equipMent.OnHeadStat?.Invoke();
                }

            }



        }
        else if (QuickEquipmentSlotStay) //�׷��������� �����â�� �����ִ»��¸�
        {
            var ArmorSlot = UIManager.instance.equipMent.ArmorSlot;
            var BootsSlot = UIManager.instance.equipMent.BootsSlot;
            var HeadSlot = UIManager.instance.equipMent.HeadSlot;

            if (GetComponent<Armor>())
            {
                SlotIn = true;
                transform.rotation = Quaternion.identity;
                QuickEquipmentSlotStay = false;
                if (GetComponent<Armor>().Type == UIManager.instance.Chest && ArmorSlot.itemObject == null)
                {
                    slot = ArmorSlot;
                   
                    
                    ScaleSizeDown(); //������ �̵��ϸ� ũ�����̱�
                    if (gameObject == UIInteractor.instance.LeftGrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.LeftGrabInteractableObject);
                    }
                    else if (gameObject == UIInteractor.instance.GrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.GrabInteractableObject);
                    }
                    UIManager.instance.equipMent.OnArmorStat?.Invoke();
                }
                else if (GetComponent<Armor>().Type == UIManager.instance.Boots && BootsSlot.itemObject == null)
                {
                    slot = BootsSlot;

                   
                    ScaleSizeDown(); //������ �̵��ϸ� ũ�����̱�
                    if (gameObject == UIInteractor.instance.LeftGrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.LeftGrabInteractableObject);
                    }
                    else if (gameObject == UIInteractor.instance.GrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.GrabInteractableObject);
                    }
                    UIManager.instance.equipMent.OnBootsStat?.Invoke();
                }
                else if (GetComponent<Armor>().Type == UIManager.instance.Head && HeadSlot.itemObject == null)
                {
                    slot = HeadSlot;

                  
                    ScaleSizeDown(); //������ �̵��ϸ� ũ�����̱�
                    if (gameObject == UIInteractor.instance.LeftGrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.LeftGrabInteractableObject);
                    }
                    else if (gameObject == UIInteractor.instance.GrabInteractableObject)
                    {
                        slot.AddItem(UIInteractor.instance.GrabInteractableObject);
                    }
                    UIManager.instance.equipMent.OnHeadStat?.Invoke();
                }
               
            }
           
        }
        UIManager.instance.toolTip.ToolTipOff(); //�׷��ɶ� ������ �����ϱ�����

    }

    public void InitSlot()
    {
        slot = null;
    }

    public void ScaleSizeUp()
    {
        scale.DOScale(originScale, 0.3f).SetEase(Ease.Linear);
    }
    public void ScaleSizeDown()
    {
        scale.DOScale(smallScale, 0.3f).SetEase(Ease.Linear);
    }

    public void PositionInit()
    {
        scale.localPosition = originPosition;
    }
}

