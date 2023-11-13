using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [Header("UI ��ũ��Ʈ")]
    public ToolTip toolTip; //���� �Ҵ��ϱ�. ����������ȴ�ϴ� ���� �ۿ��־
    public StatUI statUi;
    public EquipMent equipMent;
    [Header("UI �г�")]
    public GameObject MainMenuPanel;
    public GameObject GameUI;
    public GameObject MapPanel;
    public GameObject CharacterPanel;
    public GameObject InventoryPanel;
    public GameObject OptionPanel;
    public GameObject TooltipPanel; //���� �Ҵ��ϱ�. ����������ȴ�ϴ� ���� �ۿ��־
    public GameObject SkillGuidePanel;

    [Header("��������Ʈ �̹���")]
    public Sprite slotDefaultSprite; //������ �⺻��������Ʈ (�Ķ���)
    public Sprite slotContactSprite; //������ �����۰� ��Ƽ� �ٲ� ��������Ʈ (������)
    public Sprite slotSkillDefaultSprite; //��ų������ �⺻��������Ʈ
    public Sprite slotWeaponDefaultSprite; //ĳ����â�� ���⽽���� �⺻��������Ʈ
    public Sprite slotFrameDefaultSprite; //������ �⺻ �׵θ� ��������Ʈ


    //��&���� Ÿ�Ե�
    public string Head = "����";
    public string Chest = "����";
    public string Boots = "��ȭ";
    public string Weapon = "����";


    [Header("---------------��Ÿ")]
    public Slot[] inventorySlots; //�κ����Ե�
    public Slot[] equipMentSlots; //���â ���Ե�
    public GameObject[] panels; //��,ĳ����,�κ��丮,�ɼ�,��ų���̵� �г�
    public Stat stat;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(UIReloading());
    }

    private void Start()
    {
        //toolTip = FindObjectOfType<ToolTip>(true);
        //TooltipPanel = toolTip.gameObject;
        //transform.SetParent(GameObject.Find("Hand_L").transform);
    }

    public bool PanelsActive() //���� ��� �г��� �����ִ°�? true�� ��
    {
        var panels = GameUI.GetComponentsInChildren<PanelAnimation>();

        foreach (var panel in panels)
        {
            if (panel.gameObject.activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator UIReloading() //��ó���� �ѹ� �״پ�Ű�� �η��۷��� ��������. 
        //���� ã������1�� ��� ������ �����ϸ� ���ݾ�����Ʈ�� �ȵȴ�. �ٵ� �ѹ��̶� ĳ�����г��� �״ٲ����¸� �ȴ�.
    {
        CharacterPanel.SetActive(true);
        InventoryPanel.SetActive(true);
        SkillGuidePanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        CharacterPanel.SetActive(false);
        InventoryPanel.SetActive(false);
        SkillGuidePanel.SetActive(false);
    }

    public Color ItemGradeColor(GameObject obj) //����  ���Ϲޱ�
    {
        if (obj.GetComponent<Weapon>())
        {
            if (obj.GetComponent<Weapon>().Rank == "����")
                return new Color(1.0f, 0.647f, 0.145f); // ��Ȳ��
            else if (obj.GetComponent<Weapon>().Rank == "���")
                return Color.yellow;
            else
                return Color.white;
        }
        else
        {
            if (obj.GetComponent<Armor>().Rank == "����")
                return new Color(1.0f, 0.647f, 0.145f); // ��Ȳ��
            else if (obj.GetComponent<Armor>().Rank == "���")
                return Color.yellow;
            else
                return Color.white;
        }
    }
}
