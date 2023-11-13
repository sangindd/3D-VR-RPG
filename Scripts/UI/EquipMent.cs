using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipMent : MonoBehaviour
{
    public Slot weaponSlot;

    [SerializeField]
    GameObject itemObject; //������
    [SerializeField]
    GameObject itemObject2; //�޼�
    [SerializeField]
    GameObject HeadObject; //�ӽ÷� �����س��� �Ӹ� ������Ʈ
    [SerializeField]
    GameObject ArmorObject; //�ӽ÷� �����س��� ���ʿ�����Ʈ
    [SerializeField]
    GameObject BootsObject; //�ӽ÷� �����س��� ���� ������Ʈ
 

    //���� ����
    int s, d, i;
    float dmg;
    [SerializeField] Stat stat;
    [SerializeField] bool isEquipment = false;

    //�� ����
    public Slot ArmorSlot;
    public Slot BootsSlot;
    public Slot HeadSlot;


    //��� ���� �� �߻� �̺�Ʈ(UI ����)
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
            if (itemObject.GetComponent<Weapon>() && !isEquipment)  //���⸦ �������� ���⽽�Ժ�ȭ�� ���ݺ�ȭ
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

    IEnumerator WeaponSlotUpdatePlus() //���� �׷��� �������⸦ ���⽽�Կ� ���Ž�Ű��
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
