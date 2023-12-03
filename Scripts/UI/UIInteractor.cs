using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using DG.Tweening;
public class UIInteractor : MonoBehaviour
{
    public static UIInteractor instance;

    public InputActionReference MButtonAction; //메뉴버튼입니다
    public InputActionReference YButtonAction;
    public InputActionReference XButtonAction;
    public InputActionReference GripAction;
    public InputActionReference TriggerAction;

    public XRDirectInteractor GrabDirectInteractor;
    public XRDirectInteractor LeftGrabDirectInteractor;
    public XRDirectInteractor UIDirectInteractor;
    public XRRayInteractor UIRayInteractor; //상점구역에 들어가거나 UI열리면 나오는 레이


    public Transform player;
    [SerializeField] Transform grabControllerOriginPosition;

    public GameObject GrabInteractableObject; //현재 그랩한 오브젝트
    public GameObject LeftGrabInteractableObject; //현재 그랩한 왼쪽오브젝트

    public bool isGrabControllerMove; //그랩컨트롤러가 UI컨트롤러쪽으로 이동해야할때
    public bool isMoving; //비정상적인 반복이동을 제한하기위해

    public Transform RightHandToolTipPos; //오른손의 툴팁위치부모용

    public Transform colliderStartPoint; //UI 인터렉터의 콜라이더 시작점과 끝점. 캡슐콜라이더가 생길곳
    public Transform colliderEndPoint;

    [SerializeField]
    Npc npc; //닿은 엔피시정보 감지용

    // Sound


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

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        MButtonAction.action.Enable();
        MButtonAction.action.performed += UISkillOpen;
        XButtonAction.action.Enable();
        XButtonAction.action.performed += UIInventoryOpen;
        YButtonAction.action.Enable();
        YButtonAction.action.performed += UICharacterOpen;
        TriggerAction.action.Enable();
        TriggerAction.action.performed += InteractionUi;
    }

    private void OnDisable()
    {
        MButtonAction.action.Disable();
        MButtonAction.action.performed -= UISkillOpen;
        XButtonAction.action.Enable();
        XButtonAction.action.performed += UIInventoryOpen;
        YButtonAction.action.Enable();
        YButtonAction.action.performed += UICharacterOpen;
        TriggerAction.action.Disable();
        TriggerAction.action.performed -= InteractionUi;
    }
    private void Update()
    {
        Cursor.visible = false; //테스트할때 커서가리는용도

        //Collider[] colliders = Physics.OverlapSphere(player.transform.position, 1.7f, LayerMask.GetMask("NPC"));

        //if (colliders.Length > 0)//근처에 npc가있으면 레이를킨다
        //{
        //    UIRayInteractor.enabled = true;
        //}
        //else
        //{
        //    UIRayInteractor.enabled = false;
        //}

        #region 그랩한 오브젝트 갱신
        var selectedInteractables = GrabDirectInteractor.interactablesSelected;
        if (selectedInteractables.Count > 0)
        {
            foreach (var interactable in selectedInteractables)
            {
                GrabInteractableObject = interactable.transform.gameObject;
            }
        }
        else
        {
            GrabInteractableObject = null;
        }
        #endregion

        #region 그랩한 오브젝트 갱신 왼손
        var leftselectedInteractables = LeftGrabDirectInteractor.interactablesSelected;
        if (leftselectedInteractables.Count > 0)
        {
            foreach (var interactable in leftselectedInteractables)
            {
                LeftGrabInteractableObject = interactable.transform.gameObject;
            }
        }
        else
        {
            LeftGrabInteractableObject = null;
        }
        #endregion

        GrabControllerSetting();
    }


    public void InteractionUi(InputAction.CallbackContext obj) //다이렉트 인터렉터로 버튼을 상호작용 / 트리거 이벤트
    {
        Collider[] colliders = Physics.OverlapCapsule(colliderStartPoint.position, colliderEndPoint.position, 0.015f);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<Button>())
                {
                    colliders[i].GetComponent<Button>().onClick.Invoke();
                    //break;
                }
            }
        }
    }


    //private void OnDrawGizmos() // 씬에서 위 메서드의 범위확인
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(UIDirectInteractor.transform.position, 0.02f);
    //}


    private void UIInventoryOpen(InputAction.CallbackContext obj) //X키 인벤 
    {
        // UI Open
        if (!UIManager.instance.InventoryPanel.activeSelf)
        {
            // Sound
            SoundManager.Instance.PlayUISoundOneShot("UI_Open");

            if (!UIManager.instance.PanelsActive())
            {
                var panels = UIManager.instance.GameUI.GetComponentsInChildren<PanelAnimation>();
                for (int i = 0; i < panels.Length; i++)
                {
                    if (panels[i].gameObject.activeSelf)
                    {
                        panels[i].gameObject.SetActive(false);
                    }
                }
            }

            UIManager.instance.InventoryPanel.SetActive(true);

        }
        else  //모든 패널 끄기
        {
            // Sound
            SoundManager.Instance.PlayUISoundOneShot("UI_Close");

            var panels = UIManager.instance.GameUI.GetComponentsInChildren<PanelAnimation>();
            UIManager.instance.MainMenuPanel.GetComponent<PanelAnimation>().ScaleDown();
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].gameObject.activeSelf)
                {
                    panels[i].ScaleDown();
                }
            }
        }
    }
    private void UICharacterOpen(InputAction.CallbackContext obj) //Y키 캐릭터창
    {
        // UI Open
        if (!UIManager.instance.CharacterPanel.activeSelf)
        {
            // Sound
            SoundManager.Instance.PlayUISoundOneShot("UI_Open");

            if (!UIManager.instance.PanelsActive())
            {
                var panels = UIManager.instance.GameUI.GetComponentsInChildren<PanelAnimation>();
                for (int i = 0; i < panels.Length; i++)
                {
                    if (panels[i].gameObject.activeSelf)
                    {
                        panels[i].gameObject.SetActive(false);
                    }
                }
            }

            UIManager.instance.CharacterPanel.SetActive(true);
        }
        else  //모든 패널 끄기
        {
            // Sound
            SoundManager.Instance.PlayUISoundOneShot("UI_Close");

            var panels = UIManager.instance.GameUI.GetComponentsInChildren<PanelAnimation>();
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].gameObject.activeSelf)
                {
                    panels[i].ScaleDown();
                }
            }
        }
    }

    private void UISkillOpen(InputAction.CallbackContext obj) //메뉴키 스킬
    {
        // UI Open
        if (!UIManager.instance.SkillGuidePanel.activeSelf)
        {
            // Sound
            SoundManager.Instance.PlayUISoundOneShot("UI_Open");
            if (!UIManager.instance.PanelsActive())
            {
                var panels = UIManager.instance.GameUI.GetComponentsInChildren<PanelAnimation>();
                for (int i = 0; i < panels.Length; i++)
                {
                    if (panels[i].gameObject.activeSelf)
                    {
                        panels[i].gameObject.SetActive(false);
                    }
                }
            }
            UIManager.instance.SkillGuidePanel.SetActive(true);
        }
        else  //모든 패널 끄기
        {
            // Sound
            SoundManager.Instance.PlayUISoundOneShot("UI_Close");

            var panels = UIManager.instance.GameUI.GetComponentsInChildren<PanelAnimation>();
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].gameObject.activeSelf)
                {
                    panels[i].ScaleDown();
                }
            }
        }
    }


    public void GrabControllerSetting() //슬롯 아이템을 꺼낼때 UI컨트롤러가 닿았을때 그랩컨트롤러를 UI컨트롤러쪽으로 이동시키기
    {
        if (isGrabControllerMove && !isMoving)
        {
            GrabDirectInteractor.transform.position = UIDirectInteractor.transform.position;
            //GrabDirectInteractor.transform.DOMove(UIDirectInteractor.transform.position, 0.1f).OnComplete(() =>
            //{
            //    isMoving = false;
            //});
        }
        else
        {
            GrabDirectInteractor.transform.position = grabControllerOriginPosition.position;
            //GrabDirectInteractor.transform.DOMove(grabControllerOriginPosition.position, 0.1f).OnComplete(() =>
            //{
            //    isMoving = false;
            //});
        }
    }

    public void GrabControllerBoolCheck(bool check)
    {
        isGrabControllerMove = check;
    }

}
