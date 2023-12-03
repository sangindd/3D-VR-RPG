using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class CharacterSelect : MonoBehaviour
{
    public enum Type
    {
        Knight,
        Sorcerer
    }

    public Type jobType;

    [SerializeField] Player player;
    [SerializeField] SelectToolTip selectTooltip;
    [SerializeField] Transform toolTipPos; //툴팁이 생길곳

    [SerializeField] string className; //클래스이름
    [SerializeField] string classExplanation; //클래스설명


    private void Awake()
    {
        player = FindObjectOfType<Player>();
        //selectTooltip = FindObjectOfType<SelectToolTip>(true);
    }


    public void SelectJob(int t) //직업 선택
    {
        player.SetType(t);
    }


    public void HoverEvent(string job) //캐릭터를 가르켰을때
    {
        if (job == "Knight")  //가르킨 캐릭터가 나이트면
        {
            selectTooltip.classImage.sprite = selectTooltip.knightClassImage;
            selectTooltip.classNameText.text = "나이트";
            selectTooltip.classExpText.text = "나이트는 근접공격을 위주로 하는 직업입니다.  위험에 노출되는 시간이 많은 만큼 내구력이       좋습니다.";
            for (int i = 0; i < selectTooltip.skillImages.Length; i++)
            {
                selectTooltip.skillImages[i].sprite = selectTooltip.knightSkills[i];
            }
        }
        else  //가르킨 캐릭터가 소서러면
        {
            selectTooltip.classImage.sprite = selectTooltip.sorcererClassImage;
            selectTooltip.classNameText.text = "소서러";
            selectTooltip.classExpText.text = "소서러는 원거리 마법을 주로 쓰는 직업입니다. 몬스터를 먼 거리에서 공격할 수 있지만 내구력이 약한 단점이 있습니다.";
            for (int i = 0; i < selectTooltip.skillImages.Length; i++)
            {
                selectTooltip.skillImages[i].sprite = selectTooltip.sorcererSkills[i];
            }
        }
        selectTooltip.anim = GetComponent<Animator>();
        //툴팁의 위치설정
        //selectTooltip.transform.position = toolTipPos.position;
        //selectTooltip.transform.rotation = toolTipPos.rotation;
    }
}
