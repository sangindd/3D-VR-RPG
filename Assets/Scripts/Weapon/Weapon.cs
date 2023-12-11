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
        this.Name_Skill = Name_Skill;         //스킬 데미지
        this.Info = Info;                        //무기 설명
        this.Info_Skill = Info_Skill;            //스킬 설명
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
    [SerializeField] public Sprite Sprite; //인벤에 들어갈 2D 오브젝트의 이미지
    [SerializeField] public Sprite[] Skill_Sprite; //무기의 스킬 이미지
    [SerializeField] public VideoClip[] Skill_Videos; //무기가 가지고있는 스킬비디오들, 위  스킬이미지와 서순맞추기

    [SerializeField]
    public int Price = 0;
}
