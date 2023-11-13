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
    [SerializeField] Transform toolTipPos; //������ �����

    [SerializeField] string className; //Ŭ�����̸�
    [SerializeField] string classExplanation; //Ŭ��������


    private void Awake()
    {
        player = FindObjectOfType<Player>();
        //selectTooltip = FindObjectOfType<SelectToolTip>(true);
    }


    public void SelectJob(int t) //���� ����
    {
        player.SetType(t);
    }


    public void HoverEvent(string job) //ĳ���͸� ����������
    {
        if (job == "Knight")  //����Ų ĳ���Ͱ� ����Ʈ��
        {
            selectTooltip.classImage.sprite = selectTooltip.knightClassImage;
            selectTooltip.classNameText.text = "����Ʈ";
            selectTooltip.classExpText.text = "����Ʈ�� ���������� ���ַ� �ϴ� �����Դϴ�.  ���迡 ����Ǵ� �ð��� ���� ��ŭ ��������       �����ϴ�.";
            for (int i = 0; i < selectTooltip.skillImages.Length; i++)
            {
                selectTooltip.skillImages[i].sprite = selectTooltip.knightSkills[i];
            }
        }
        else  //����Ų ĳ���Ͱ� �Ҽ�����
        {
            selectTooltip.classImage.sprite = selectTooltip.sorcererClassImage;
            selectTooltip.classNameText.text = "�Ҽ���";
            selectTooltip.classExpText.text = "�Ҽ����� ���Ÿ� ������ �ַ� ���� �����Դϴ�. ���͸� �� �Ÿ����� ������ �� ������ �������� ���� ������ �ֽ��ϴ�.";
            for (int i = 0; i < selectTooltip.skillImages.Length; i++)
            {
                selectTooltip.skillImages[i].sprite = selectTooltip.sorcererSkills[i];
            }
        }
        selectTooltip.anim = GetComponent<Animator>();
        //������ ��ġ����
        //selectTooltip.transform.position = toolTipPos.position;
        //selectTooltip.transform.rotation = toolTipPos.rotation;
    }
}
