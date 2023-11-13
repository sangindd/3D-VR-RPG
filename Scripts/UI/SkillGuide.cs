using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
public class SkillGuide : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI skillName;  //스킬의 이름
    [SerializeField]
    private TextMeshProUGUI skillExp;  //스킬 설명
    [SerializeField]
    private TextMeshProUGUI skillDmg; //스킬 데미지


    [SerializeField]
    private Sprite skillSelectImage; //현재 선택되어있는 스킬의 프레임 스프라이트
    [SerializeField]
    private Image skill_1_Slot_Frame; //스킬1의 슬롯프레임
    [SerializeField]
    private Image skill_2_Slot_Frame; //스킬2의 슬롯프레임


    [SerializeField]     //스킬창의 스킬이미지 슬롯들
    private Image skill_1_Slot_Image;
    [SerializeField]
    private Image skill_2_Slot_Image;



    [SerializeField]
    private RawImage videoRawImage; //동영상 로우이미지, 영상을 껐다켰다하기위함
    [SerializeField]
    private VideoPlayer videoPlayer; //동영상플레이어


    [SerializeField]
    VideoClip[] skill_Videos = new VideoClip[2]; //무기가 가지고있는 스킬 동영상들

    string[] Skill_Info;

    string skill1_Name = "";
    string skill1_Dmg = "";
    string skill1_Exp = "";

    string skill2_Name = "";
    string skill2_Dmg = "";
    string skill2_Exp = "";

    private void OnDisable()
    {
        GuideInit();
    }


    private void Update()
    {
        var itemObject = UIInteractor.instance.GrabInteractableObject; //손에 뭔가 든게있다면
        if (itemObject != null && itemObject.GetComponent<Weapon>() != null)
        {   //스킬할당
            Skill_Info = itemObject.GetComponent<Weapon>().Info_Skill[0].ToString().Split("<bb>");
            skill1_Name = itemObject.GetComponent<Weapon>().Name_Skill[0].ToString();
            skill1_Exp = Skill_Info[0];

            Skill_Info = itemObject.GetComponent<Weapon>().Info_Skill[1].ToString().Split("<bb>");
            skill2_Name = itemObject.GetComponent<Weapon>().Name_Skill[1].ToString();
            skill2_Exp = Skill_Info[0];

            try
            {
                var skill = itemObject.GetComponent<Staff>();
                var dmg = skill.GetComponent<Magic>().GetDmg(skill.GetComponent<Magic>().charge);
                skill1_Dmg = Skill_Info[1].Replace("dmg", dmg.ToString());
                dmg = skill.GetComponent<Magic>().GetDmg(skill.GetComponent<Magic>().ground);
                skill2_Dmg = Skill_Info[1].Replace("dmg", dmg.ToString());
            }
            catch
            {
                var skill = itemObject.GetComponent<Sword>();
                var dmg = skill.GetComponent<Slash>().Slash1.GetComponent<Damage>();
                dmg.SetDamage();
                skill1_Dmg = Skill_Info[1].Replace("dmg", dmg.GetDamage().ToString());
                dmg = skill.GetComponent<Slash>().Slash2.GetComponent<Damage>();
                dmg.SetDamage();
                skill2_Dmg = Skill_Info[1].Replace("dmg", dmg.GetDamage().ToString());
            }

            skill_1_Slot_Image.sprite = itemObject.GetComponent<Weapon>().Skill_Sprite[0]; //스킬슬롯이미지바꾸기
            skill_2_Slot_Image.sprite = itemObject.GetComponent<Weapon>().Skill_Sprite[1];
            skill_Videos[0] = itemObject.GetComponent<Weapon>().Skill_Videos[0];  //비디오클립넣기
            skill_Videos[1] = itemObject.GetComponent<Weapon>().Skill_Videos[1];
        }
        else
        {//텍스트 지우고 이미지 전부 기본으로 되돌리기.
            GuideInit();
        }
    }

    private void GuideInit() //스킬가이드를 껐을때도 초기화되게. 
    {
        skillName.text = "";
        skillExp.text = "";
        skillDmg.text = "";
        skill_1_Slot_Image.sprite = UIManager.instance.slotSkillDefaultSprite;
        skill_2_Slot_Image.sprite = UIManager.instance.slotSkillDefaultSprite;
        videoPlayer.clip = null;
        skill_Videos[0] = null;
        skill_Videos[1] = null;
        skill1_Name = "";
        skill1_Dmg = "";
        skill1_Exp = "";

        skill2_Name = "";
        skill2_Dmg = "";
        skill2_Exp = "";

        videoRawImage.enabled = false; //비디오 종료
    }

    public void SkillSlotClick(int i) //스킬슬롯의 버튼을 클릭하면 발생
    {   //직접 스킬슬롯버튼에서 1 과 2를 직접할당. 

        if (i == 1)  //1번스킬슬롯이라면 1번스킬 슬롯테두리를 눈에띄는걸로 변경, 2번스킬 테두리는 기본으로
        {            //비디오클립도 넣어주기
            videoPlayer.clip = skill_Videos[0];
            skill_1_Slot_Frame.sprite = skillSelectImage;
            skillName.text = skill1_Name;
            skillExp.text = skill1_Exp;
            skillDmg.text = skill1_Dmg;

            skill_2_Slot_Frame.sprite = UIManager.instance.slotFrameDefaultSprite;
        }
        else if (i == 2)
        {
            videoPlayer.clip = skill_Videos[1];
            skill_2_Slot_Frame.sprite = skillSelectImage;
            skillName.text = skill2_Name;
            skillExp.text = skill2_Exp;
            skillDmg.text = skill2_Dmg;

            skill_1_Slot_Frame.sprite = UIManager.instance.slotFrameDefaultSprite;
        }

        videoRawImage.enabled = true; //비디오 켜기
    }

}
