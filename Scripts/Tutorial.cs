using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    InputActionAsset action;
    [SerializeField]
    List<GameObject> tutorials;


    [Header("Controller")]
    [SerializeField]
    GameObject Left;
    [SerializeField]
    Animator animLeft;
    [SerializeField]
    GameObject Right;
    [SerializeField]
    Animator animRight;
    [SerializeField]
    Material matGrab;
    [SerializeField]
    Material matTrigger;
    [SerializeField]
    Material matXButton;
    [SerializeField]
    Material matYButton;
    [SerializeField]
    Material matMenu;
    [SerializeField]
    Material matJoyStick;

    [Header("Slot")]
    [SerializeField]
    Slot ArmorSlot;
    [SerializeField]
    Slot WeaponSlot;

    [Header("Sound")]
    [SerializeField]
    AudioSource source;
    [SerializeField]
    List<AudioClip> clips;

    bool[] tutorial = new bool[20];
    bool isChange = false;
    IEnumerator enumerator;

    private void Awake()
    {
        for (int i = 0; i < tutorial.Length; i++)
        {
            tutorial[i] = false;
        }
    }

    void Start()
    {
        action.FindActionMap("XRI LeftHand").FindAction("X Button").performed += XButton;



        StartCoroutine(Process());
    }

    void Update()
    {

    }

    void YButton(InputAction.CallbackContext obj)
    {
        animRight.SetTrigger("Idle");
        matYButton.color = Color.black;
        StartCoroutine(OnNext(3, 3f));
        action.FindActionMap("XRI LeftHand").FindAction("Y Button").performed -= YButton;
    }

    void Menu(InputAction.CallbackContext obj)
    {
        matMenu.color = Color.black;
        Next(8);
        action.FindActionMap("XRI LeftHand").FindAction("Menu").performed -= Menu;
    }

    void XButton(InputAction.CallbackContext obj)
    {
        tutorial[4] = true;
    }

    IEnumerator Process()
    {
        while (true)
        {
            yield return new WaitForSeconds(.2f);
            if (!tutorial[0])
            {
                source.clip = clips[0];
                source.Play();
                animRight.SetTrigger("Grab");
                matGrab.color = Color.blue;
                yield return new WaitForSeconds(7f);
                tutorials[0].SetActive(false);
                tutorial[0] = true;
            }
            else if (!tutorial[1])
            {
                tutorials[1].SetActive(true);
            }
            else if (!tutorial[2])
            {
                animRight.SetTrigger("Idle");
                //이동방법 설명
                if (!isChange)
                {
                    source.clip = clips[1];
                    source.Play();
                    isChange = true;
                }
                matGrab.color = Color.white;
                matJoyStick.color = Color.blue;
                tutorials[2].SetActive(true);
                var dst = Vector3.Distance(FindObjectOfType<XROrigin>().transform.position, tutorials[2].transform.position);
                if (dst <= 5f)
                {
                    //if(!isChange)
                    //    ChangeColor(matYButton, Color.blue);
                    //isChange = true;
                    matJoyStick.color = Color.black;
                    tutorials[2].SetActive(false);
                    tutorial[2] = true;
                    isChange = false;
                }
            }
            else if (!tutorial[3])
            {
                //캐릭터창 설명
                if (!isChange)
                {
                    source.clip = clips[2];
                    source.Play();
                    isChange = true;
                    matYButton.color = Color.blue;
                    action.FindActionMap("XRI LeftHand").FindAction("Y Button").performed += YButton;
                }
            }
            else if (!tutorial[4])
            {
                //퀵장착 설명
                if (!isChange)
                {
                    source.clip = clips[3]; //3
                    enumerator = PlaySound();
                    StartCoroutine(enumerator);
                    isChange = true;
                }
                tutorials[3].SetActive(true);
                if (ArmorSlot.itemObject != null) //장착 완료시
                {
                    //ArmorSlot.itemObject.transform.SetParent(null);
                    StopCoroutine(enumerator);
                    tutorials[3].SetActive(false);
                    Next(4);
                }
            }
            else if (!tutorial[5])  //캐릭터창 확인
            {
                if (!isChange)
                {
                    source.clip = clips[4]; //4
                    source.Play();
                    isChange = true;
                    StartCoroutine(OnNext(5, 5f));
                }
            }
            else if (!tutorial[6])  //검 설명
            {
                //기초설명 + 툴팁
                if (!isChange)
                {
                    source.clip = clips[5]; //5
                    source.Play();
                    matGrab.color = Color.blue;
                    animRight.SetTrigger("Grab");
                    isChange = true;
                }
                tutorials[4].SetActive(true);
                tutorials[9].SetActive(false);
                if (WeaponSlot.itemObject != null)  //무기 그립시
                {
                    Next(6);
                    animRight.SetTrigger("Idle");
                    matGrab.color = Color.white;
                }
            }
            else if (!tutorial[7])  //검 로직 설명
            {
                if (!isChange)
                {
                    source.clip = clips[6]; //6
                    source.Play();
                    isChange = true;
                    StartCoroutine(OnNext(7, 10f));
                }
            }
            else if (!tutorial[8])  //스킬 창 확인
            {
                if (!isChange)
                {
                    source.clip = clips[7]; //7
                    source.Play();
                    isChange = true;
                    matMenu.color = Color.blue;
                    action.FindActionMap("XRI LeftHand").FindAction("Menu").performed += Menu;
                }
            }
            else if (!tutorial[9])
            {
                if (!isChange)
                {
                    source.clip = clips[8]; //8
                    source.Play();
                    isChange = true;
                }
            }
        }
    }

    IEnumerator ChangeColor(Material obj, Color color)
    {
        while (true)
        {
            obj.color = Color.Lerp(obj.color, color, 0.5f);
            //obj.transform.GetChild(0).GetComponent<Material>().
            yield return new WaitForSeconds(0.5f);
            obj.color = Color.Lerp(obj.color, Color.white, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator PlaySound()
    {
        while (true)
        {
            source.Play();
            yield return new WaitForSeconds(10f);
        }
    }

    IEnumerator OnNext(int num, float delay)
    {
        yield return new WaitForSeconds(delay);
        Next(num);
    }

    public void Next(int num)
    {
        tutorial[num] = true;
        isChange = false;
    }
}
