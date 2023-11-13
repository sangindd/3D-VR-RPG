using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
public class MonsterHpBar : MonoBehaviour
{
    [SerializeField] private GameObject HpbarFrame; //프레임
    [SerializeField] private Image Hpbar; 
    [SerializeField] private Transform player; //메인카메라를 넣어야합니다

    [SerializeField] Hp monsterHp;

    private void Awake()
    {
        player = Camera.main.transform;
        monsterHp = GetComponentInParent<Hp>();
    }

    private void Update()
    {
        Quaternion playerRotation = player.rotation; 
        playerRotation.eulerAngles = new Vector3(0, playerRotation.eulerAngles.y, 0); 
        HpbarFrame.transform.rotation = playerRotation; 
    }

    public void HpBarUpdate()
    {
        Hpbar.fillAmount = monsterHp.CurrentHp / monsterHp.MaxHp;
        if (monsterHp.CurrentHp <= 0) 
        {
            Hpbar.fillAmount = 0;
        }
    }
}
