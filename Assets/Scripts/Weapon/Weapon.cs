using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class Weapon : MonoBehaviour
{
    public Weapon()
    {
    }
    public Weapon(string ItemName, string Type, string Rank, string Strength, string Dexterity, string Intelligence, string Damage_Basic, List<string> Name_Skill, string Info, List<string> Info_Skill)
    {
        this.ItemName = ItemName;
        this.Type = Type;
        this.Rank = Rank;
        this.Strength = Strength;
        this.Dexterity = Dexterity;
        this.Intelligence = Intelligence;
        this.Damage_Basic = Damage_Basic;
        this.Name_Skill = Name_Skill;         //��ų ������
        this.Info = Info;                        //���� ����
        this.Info_Skill = Info_Skill;            //��ų ����
    }

    [SerializeField] public string ItemName;
    [SerializeField] public string Type;
    [SerializeField] public string Rank;
    [SerializeField] public string Strength;
    [SerializeField] public string Dexterity;
    [SerializeField] public string Intelligence;
    [SerializeField] public string Damage_Basic;
    [SerializeField] public List<string> Name_Skill;
    [SerializeField] public string Info;
    [SerializeField] public List<string> Info_Skill;
    [SerializeField] public Sprite Sprite; //�κ��� �� 2D ������Ʈ�� �̹���
    [SerializeField] public Sprite[] Skill_Sprite; //������ ��ų �̹���
    [SerializeField] public VideoClip[] Skill_Videos; //���Ⱑ �������ִ� ��ų������, ��  ��ų�̹����� �������߱�

    [SerializeField]
    public int Price = 0;
}
