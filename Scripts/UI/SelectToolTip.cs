using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SelectToolTip : MonoBehaviour
{
    public Image classImage; //���� ǥ�õ� Ŭ������¡ �̹���
    public Sprite knightClassImage;
    public Sprite sorcererClassImage;

    public TextMeshProUGUI classNameText;  //Ŭ���� �̸� �ؽ�Ʈ
    public TextMeshProUGUI classExpText; //Ŭ���� ���� �ؽ�Ʈ

    public Image[] skillImages; //��ų�̹��� ������Ʈ

    public Sprite[] knightSkills;  //�� ������ ��ų ��������Ʈ �̹���
    public Sprite[] sorcererSkills;

    public Animator anim; //����Ų ����� �ִϸ�����



    public void Animation(string name) //��ų���� �ٸ� �ִϸ��̼��� ���� �����Ϳ��� �Ҵ����ֱ�
    {
        anim.SetTrigger(name);
    }
}
