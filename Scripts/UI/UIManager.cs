using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [Header("UI 스크립트")]
    public ToolTip toolTip; //직접 할당하기. 프리펩저장안댑니다 툴팁 밖에있어서
    public StatUI statUi;
    public EquipMent equipMent;
    [Header("UI 패널")]
    public GameObject MainMenuPanel;
    public GameObject GameUI;
    public GameObject MapPanel;
    public GameObject CharacterPanel;
    public GameObject InventoryPanel;
    public GameObject OptionPanel;
    public GameObject TooltipPanel; //직접 할당하기. 프리펩저장안댑니다 툴팁 밖에있어서
    public GameObject SkillGuidePanel;

    [Header("스프라이트 이미지")]
    public Sprite slotDefaultSprite; //슬롯의 기본스프라이트 (파랑색)
    public Sprite slotContactSprite; //슬롯이 아이템과 닿아서 바뀐 스프라이트 (빨간색)
    public Sprite slotSkillDefaultSprite; //스킬슬롯의 기본스프라이트
    public Sprite slotWeaponDefaultSprite; //캐릭터창의 무기슬롯의 기본스프라이트
    public Sprite slotFrameDefaultSprite; //슬롯의 기본 테두리 스프라이트


    //방어구&무기 타입들
    public string Head = "투구";
    public string Chest = "갑옷";
    public string Boots = "장화";
    public string Weapon = "무기";


    [Header("---------------기타")]
    public Slot[] inventorySlots; //인벤슬롯들
    public Slot[] equipMentSlots; //장비창 슬롯들
    public GameObject[] panels; //맵,캐릭터,인벤토리,옵션,스킬가이드 패널
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

    public bool PanelsActive() //현재 모든 패널이 꺼져있는가? true면 예
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

    IEnumerator UIReloading() //맨처음에 한번 켰다안키면 널레퍼런스 오류가뜸. 
        //현재 찾은오류1개 장비를 퀵장비로 장착하면 스텟업데이트가 안된다. 근데 한번이라도 캐릭터패널을 켰다끈상태면 된다.
    {
        CharacterPanel.SetActive(true);
        InventoryPanel.SetActive(true);
        SkillGuidePanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        CharacterPanel.SetActive(false);
        InventoryPanel.SetActive(false);
        SkillGuidePanel.SetActive(false);
    }

    public Color ItemGradeColor(GameObject obj) //색깔  리턴받기
    {
        if (obj.GetComponent<Weapon>())
        {
            if (obj.GetComponent<Weapon>().Rank == "전설")
                return new Color(1.0f, 0.647f, 0.145f); // 주황색
            else if (obj.GetComponent<Weapon>().Rank == "희귀")
                return Color.yellow;
            else
                return Color.white;
        }
        else
        {
            if (obj.GetComponent<Armor>().Rank == "전설")
                return new Color(1.0f, 0.647f, 0.145f); // 주황색
            else if (obj.GetComponent<Armor>().Rank == "희귀")
                return Color.yellow;
            else
                return Color.white;
        }
    }
}
