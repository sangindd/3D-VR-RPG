using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Armor : MonoBehaviour
{
    public Armor()
    {
    }
    public Armor(string ItemName,string Type,string Rank, string Hp,string Defense, string Strength, string Dexterity, string Intelligence, string Info)
    {
        this.ItemName = ItemName;
        this.Type = Type;
        this.Rank = Rank;
        this.Hp = Hp;
        this.Defense = Defense;
        this.Strength = Strength;
        this.Dexterity = Dexterity;
        this.Intelligence = Intelligence;
        this.Info = Info;                        //방어구 설명
    }

    public string ItemName;
    public string Type;
    public string Rank;
    public string Hp;
    public string Defense;
    public string Strength;
    public string Dexterity;
    public string Intelligence;
    public string Info;
    [SerializeField] public Sprite Sprite; //인벤에 들어갈 2D 오브젝트의 이미지

    [SerializeField]
    public int Price = 0;
}
