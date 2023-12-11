using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShopSlotText : MonoBehaviour, IPointerEnterHandler
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;

    public ToolTip tooltip;

    public UnityEvent OnHover;

    private void Start()
    {

    }

    public void TextSetting(GameObject item)
    {
        if (item.GetComponent<Weapon>() != null)
        {
            var w = item.GetComponent<Weapon>();
            nameText.text = w.GetComponent<Weapon>().ItemName;
            nameText.color = UIManager.instance.ItemGradeColor(w.gameObject);
            priceText.text = 0.ToString();
        }
        else if(item.GetComponent<Armor>() != null)
        {
            var a = item.GetComponent<Armor>();
            nameText.text = a.GetComponent<Armor>().ItemName;
            nameText.color = UIManager.instance.ItemGradeColor(a.gameObject);
            priceText.text = 0.ToString();
        }
    }

    public void ToolTipSetting(GameObject slot) //�� Ŭ���� ������ ������ ����
    {                         //���⼭ �Ű������� �� ������ �̺�Ʈ���� �ڱ��ڽ��� ������
        tooltip.ToolTipOn(slot);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover?.Invoke();
    }
}
