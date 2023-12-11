using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Magic : MonoBehaviour
{
    public GameObject charge;
    public GameObject ground;
    public float delay_ground;
    public float Init_Cooldown;
    public float cooldown;
    float cdr;

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        cdr = player.cdr;
        Init_Cooldown = cooldown;
    }

    private void Update()
    {
        if (cdr != player.cdr)
        {
            cooldown = Init_Cooldown - Init_Cooldown * (player.cdr / 100);
            cdr = player.cdr;
        }

    }
    
    public float GetDmg(GameObject skill)
    {
        var dmg = skill.GetComponent<Damage>();
        dmg.SetDamage();
        return dmg.GetDamage();
    }
}
