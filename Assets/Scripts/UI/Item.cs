using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;
public class Item : MonoBehaviour
{
    public bool SlotIn; //현재 슬롯안에있는상태인가?
    public bool SlotStay; //슬롯에 들어갈수있는상태인가
    public bool QuickEquipmentSlotStay; //퀵장비슬롯에 들어갈수있는상태인가?
    //public Transform ScaleObject; //크기조절용 객체 (모델을넣기)

    [SerializeField]
    private Vector3 originScale; //기본스케일
    private Vector3 originPosition;
    [SerializeField]
    private Vector3 smallScale; //바뀐후의 스케일

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
        //---------------------툴팁 생성-------------------------//
        if (other.CompareTag("GrabDirectController") &&  //툴팁생성
            UIInteractor.instance.GrabInteractableObject != gameObject &&//현재 들고있는 아이템이 내가아닐때만.
            !UIManager.instance.TooltipPanel.activeSelf && //툴팁이 꺼져있을때만 실행.
             !SlotIn)
        {
            UIManager.instance.toolTip.ToolTipOn(gameObject);
        }
        //----------------------------------------------------------//

        //---------------------장비 인벤보내기-------------------------//
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

        //---------------------장비 퀵장착-------------------------//
        if (other.CompareTag("EquipmentQuick") && !SlotIn && !GetComponent<Weapon>())
        {
            QuickEquipmentSlotStay = true;
        }
        //----------------------------------------------------------//
    }

    private void OnTriggerExit(Collider other)
    {
        InitSlot();

        if (other.CompareTag("GrabDirectController")) //다이렉트 컨트롤러가 벗어나면 툴팁 제거
        {
            UIManager.instance.toolTip.ToolTipOff();
        }

        //---------------------장비 퀵장착-------------------------//
        if (other.CompareTag("EquipmentQuick"))
        {
            QuickEquipmentSlotStay = false;
        }
        //----------------------------------------------------------//


        if (other.CompareTag("CharacterUI")) //인벤창에서 나왔을경우 그랩컨트롤러를 제자리로
        {
            //UIInteractor.instance.GrabControllerBoolCheck(false);
        }
    }

    public void InventoryPlay() //그랩인터렉터블 이벤트안에넣음
    {
        if (slot != null)
        {
            var ArmorSlot = UIManager.instance.equipMent.ArmorSlot;
            var BootsSlot = UIManager.instance.equipMent.BootsSlot;
            var HeadSlot = UIManager.instance.equipMent.HeadSlot;

            if (slot.GetComponent<Slot>().itemObject == null) //슬롯이 비어져있어야한다
            {
                if (slot == ArmorSlot && GetComponent<Armor>().Type ==
                    UIManager.instance.Chest) //갑바면 장비장착
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
                    UIManager.instance.Boots)//부츠면
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
                    UIManager.instance.Head)//헤드면
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
                else if (slot != ArmorSlot && slot != BootsSlot && slot != HeadSlot)//인벤
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
                ScaleSizeDown(); //퀵으로 이동하면 크기줄이기
                transform.rotation = Quaternion.identity;
            }

            else //현재 인벤속에 있다면
            {
                slot.ClearSlot();
                PositionInit();
                SlotIn = false;
                UIManager.instance.toolTip.ToolTipOff(); //그랩될때 툴팁을 제거하기위함
                //UIManager.instance.toolTip.GetComponent<ToolTip>().ToolTipOn(gameObject);
                if (slot == ArmorSlot && GetComponent<Armor>().Type ==
                    UIManager.instance.Chest) //갑바면 장비장착해제
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
        else if (QuickEquipmentSlotStay) //그랩했을때가 퀵장비창에 들어갈수있는상태면
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
                   
                    
                    ScaleSizeDown(); //퀵으로 이동하면 크기줄이기
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

                   
                    ScaleSizeDown(); //퀵으로 이동하면 크기줄이기
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

                  
                    ScaleSizeDown(); //퀵으로 이동하면 크기줄이기
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
        UIManager.instance.toolTip.ToolTipOff(); //그랩될때 툴팁을 제거하기위함

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

