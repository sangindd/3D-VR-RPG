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
    private TextMeshProUGUI skillName;  //��ų�� �̸�
    [SerializeField]
    private TextMeshProUGUI skillExp;  //��ų ����
    [SerializeField]
    private TextMeshProUGUI skillDmg; //��ų ������


    [SerializeField]
    private Sprite skillSelectImage; //���� ���õǾ��ִ� ��ų�� ������ ��������Ʈ
    [SerializeField]
    private Image skill_1_Slot_Frame; //��ų1�� ����������
    [SerializeField]
    private Image skill_2_Slot_Frame; //��ų2�� ����������


    [SerializeField]     //��ųâ�� ��ų�̹��� ���Ե�
    private Image skill_1_Slot_Image;
    [SerializeField]
    private Image skill_2_Slot_Image;



    [SerializeField]
    private RawImage videoRawImage; //������ �ο��̹���, ������ �����״��ϱ�����
    [SerializeField]
    private VideoPlayer videoPlayer; //�������÷��̾�


    [SerializeField]
    VideoClip[] skill_Videos = new VideoClip[2]; //���Ⱑ �������ִ� ��ų �������

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
        var itemObject = UIInteractor.instance.GrabInteractableObject; //�տ� ���� ����ִٸ�
        if (itemObject != null && itemObject.GetComponent<Weapon>() != null)
        {   //��ų�Ҵ�
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

            skill_1_Slot_Image.sprite = itemObject.GetComponent<Weapon>().Skill_Sprite[0]; //��ų�����̹����ٲٱ�
            skill_2_Slot_Image.sprite = itemObject.GetComponent<Weapon>().Skill_Sprite[1];
            skill_Videos[0] = itemObject.GetComponent<Weapon>().Skill_Videos[0];  //����Ŭ���ֱ�
            skill_Videos[1] = itemObject.GetComponent<Weapon>().Skill_Videos[1];
        }
        else
        {//�ؽ�Ʈ ����� �̹��� ���� �⺻���� �ǵ�����.
            GuideInit();
        }
    }

    private void GuideInit() //��ų���̵带 �������� �ʱ�ȭ�ǰ�. 
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

        videoRawImage.enabled = false; //���� ����
    }

    public void SkillSlotClick(int i) //��ų������ ��ư�� Ŭ���ϸ� �߻�
    {   //���� ��ų���Թ�ư���� 1 �� 2�� �����Ҵ�. 

        if (i == 1)  //1����ų�����̶�� 1����ų �����׵θ��� ������°ɷ� ����, 2����ų �׵θ��� �⺻����
        {            //����Ŭ���� �־��ֱ�
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

        videoRawImage.enabled = true; //���� �ѱ�
    }

}
