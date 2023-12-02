using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipMent : MonoBehaviour
{
    public Slot weaponSlot;

    [SerializeField]
    GameObject itemObject; //오른손
    [SerializeField]
    GameObject itemObject2; //왼손
    [SerializeField]
    GameObject HeadObject; //임시로 저장해놓을 머리 오브젝트
    [SerializeField]
    GameObject ArmorObject; //임시로 저장해놓을 갑옷오브젝트
    [SerializeField]
    GameObject BootsObject; //임시로 저장해놓을 부츠 오브젝트
 

    //무기 장착
    int s, d, i;
    float dmg;
    [SerializeField] Stat stat;
    [SerializeField] bool isEquipment = false;

    //방어구 장착
    public Slot ArmorSlot;
    public Slot BootsSlot;
    public Slot HeadSlot;


    //장비 장착 시 발생 이벤트(UI 갱신)
    public UnityEvent OnEquipment;

    public UnityEvent OnArmorStat;
    public UnityEvent OnBootsStat;
    public UnityEvent OnHeadStat;



    private void Update()
    {
        itemObject = UIInteractor.instance.GrabInteractableObject;
        itemObject2 = UIInteractor.instance.LeftGrabInteractableObject;

        stat = FindObjectOfType<Player>().GetComponent<Stat>();

        if (itemObject != null)
        {
            if (itemObject.GetComponent<Weapon>() && !isEquipment)  //무기를 들었을경우 무기슬롯변화와 스텟변화
            {
                s = int.Parse(itemObject.GetComponent<Weapon>().Strength);
                d = int.Parse(itemObject.GetComponent<Weapon>().Dexterity);
                i = int.Parse(itemObject.GetComponent<Weapon>().Intelligence);
                weaponSlot.GetComponent<Slot>().AddItem(UIInteractor.instance.GrabInteractableObject);
                StartCoroutine(WeaponSlotUpdatePlus());
                isEquipment = true;
            }
        }
        else
        {
            if (isEquipment)
            {
                weaponSlot.GetComponent<Slot>().ClearSlot();
                weaponSlot.GetComponent<Slot>().itemImage.sprite = UIManager.instance.slotWeaponDefaultSprite;
                StartCoroutine(WeaponSlotUpdateMinus());
                isEquipment = false;
            }
        }
    }

    IEnumerator WeaponSlotUpdatePlus() //현재 그랩한 장착무기를 무기슬롯에 갱신시키기
    {
        stat.UpdatePlusStats(int.Parse(itemObject.GetComponent<Weapon>().Strength),
            int.Parse(itemObject.GetComponent<Weapon>().Dexterity),
            int.Parse(itemObject.GetComponent<Weapon>().Intelligence));
        dmg = int.Parse(itemObject.GetComponent<Weapon>().Damage_Basic);
        OnEquipment?.Invoke();
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator WeaponSlotUpdateMinus()
    {
        stat.UpdateMinusStats(s, d, i);
        OnEquipment?.Invoke();
        yield return new WaitForSeconds(0.2f);
    }

    public void EquipArmor()
    {
        if (ArmorSlot.itemObject != null)
        {
            // Equip Sound - Armor
            SoundManager.Instance.PlayUISoundOneShot("Sorcerer_Equip");

            ArmorObject = ArmorSlot.itemObject;
            stat.UpdatePlusStats(int.Parse(ArmorObject.GetComponent<Armor>().Strength),
          int.Parse(ArmorObject.GetComponent<Armor>().Dexterity),
          int.Parse(ArmorObject.GetComponent<Armor>().Intelligence),
          int.Parse(ArmorObject.GetComponent<Armor>().Defense));
            OnEquipment?.Invoke();
        }
        else
        {
            // Unequip Sound - Armor
            SoundManager.Instance.PlayUISoundOneShot("Sorcerer_Unequip");

            stat.UpdateMinusStats(int.Parse(ArmorObject.GetComponent<Armor>().Strength),
           int.Parse(ArmorObject.GetComponent<Armor>().Dexterity),
           int.Parse(ArmorObject.GetComponent<Armor>().Intelligence),
           int.Parse(ArmorObject.GetComponent<Armor>().Defense));
            OnEquipment?.Invoke();
            ArmorObject = null;
        }
    }

    public void EquipBoots()
    {
        if (BootsSlot.itemObject != null)
        {
            // Equip Sound - Boots
            SoundManager.Instance.PlayUISoundOneShot("Sorcerer_Equip");

            BootsObject = BootsSlot.itemObject;
            stat.UpdatePlusStats(int.Parse(BootsObject.GetComponent<Armor>().Strength),
            int.Parse(BootsObject.GetComponent<Armor>().Dexterity),
            int.Parse(BootsObject.GetComponent<Armor>().Intelligence),
            int.Parse(BootsObject.GetComponent<Armor>().Defense));
            OnEquipment?.Invoke();
        }
        else
        {
            // Unequip Sound - Boots
            SoundManager.Instance.PlayUISoundOneShot("Sorcerer_Unequip");

            stat.UpdateMinusStats(int.Parse(BootsObject.GetComponent<Armor>().Strength),
          int.Parse(BootsObject.GetComponent<Armor>().Dexterity),
          int.Parse(BootsObject.GetComponent<Armor>().Intelligence),
          int.Parse(BootsObject.GetComponent<Armor>().Defense));
            OnEquipment?.Invoke();
            BootsObject = null;
        }
    }

    public void EquipHead()
    {
        if (HeadSlot.itemObject != null)
        {
            // Equip Sound - Head
            SoundManager.Instance.PlayUISoundOneShot("Sorcerer_Equip");

            HeadObject = HeadSlot.itemObject;
            stat.UpdatePlusStats(int.Parse(HeadObject.GetComponent<Armor>().Strength),
            int.Parse(HeadObject.GetComponent<Armor>().Dexterity),
            int.Parse(HeadObject.GetComponent<Armor>().Intelligence),
            int.Parse(HeadObject.GetComponent<Armor>().Defense));
            OnEquipment?.Invoke();
        }
        else
        {
            // Unequip Sound - Head
            SoundManager.Instance.PlayUISoundOneShot("Sorcerer_Unequip");

            stat.UpdateMinusStats(int.Parse(HeadObject.GetComponent<Armor>().Strength),
          int.Parse(HeadObject.GetComponent<Armor>().Dexterity),
          int.Parse(HeadObject.GetComponent<Armor>().Intelligence),
          int.Parse(HeadObject.GetComponent<Armor>().Defense));
            OnEquipment?.Invoke();
            HeadObject = null;
        }
    }
}
