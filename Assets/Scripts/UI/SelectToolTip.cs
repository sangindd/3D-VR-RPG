using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SelectToolTip : MonoBehaviour
{
    public Image classImage; //맨위 표시될 클래스상징 이미지
    public Sprite knightClassImage;
    public Sprite sorcererClassImage;

    public TextMeshProUGUI classNameText;  //클래스 이름 텍스트
    public TextMeshProUGUI classExpText; //클래스 설명 텍스트

    public Image[] skillImages; //스킬이미지 컴포넌트

    public Sprite[] knightSkills;  //각 직업의 스킬 스프라이트 이미지
    public Sprite[] sorcererSkills;

    public Animator anim; //가르킨 대상의 애니메이터



    public void Animation(string name) //스킬마다 다른 애니메이션을 직접 에디터에서 할당해주기
    {
        anim.SetTrigger(name);
    }
}
