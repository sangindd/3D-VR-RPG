using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class MainMenus : MonoBehaviour
{
    public float MainMenuScale = 1.5f; //메인메뉴 아이콘은 크기가 너무 작아서 맞췄습니다


    [SerializeField]
    private TextMeshProUGUI mainMenuText; //중앙에 나타날 메인메뉴 설명텍스트

    [SerializeField]
    private string myName;  //자신메뉴의 이름. ex)캐릭터, 가방, 스킬설명  에디터에서 각 메뉴들마다 직접설정


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DirectController")) //메인메뉴 커질때
        {
            transform.DOScale(2f, 0.5f)
           .SetEase(Ease.OutBack);

            mainMenuText.text = myName;   //메인메뉴 텍스트 설정
        }
    }

    private void OnTriggerExit(Collider other) //메인메뉴 작아질때
    {
        if (other.CompareTag("DirectController"))
        {
            transform.DOScale(MainMenuScale, 0.5f)
           .SetEase(Ease.OutBack);

            mainMenuText.text = "";     //메인메뉴 텍스트 설정
        }
    }
}
