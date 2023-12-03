using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar2 : MonoBehaviour
{
    [SerializeField] private GameObject HpbarFrame; //프레임
    [SerializeField] private Image Hpbar;
    [SerializeField] private Transform player; //메인카메라를 넣어야 합니다

    [SerializeField] Monster_StateMachine2 monsterHp;

    private void Awake()
    {
        player = Camera.main.transform;
        monsterHp = GetComponentInParent<Monster_StateMachine2>();
    }

    private void Update()
    {
        Quaternion playerRotation = player.rotation;
        playerRotation.eulerAngles = new Vector3(0, playerRotation.eulerAngles.y, 0);
        HpbarFrame.transform.rotation = playerRotation;
    }

    public void HpBarUpdate()
    {
        Hpbar.fillAmount = monsterHp.CurrentHP / monsterHp.MaxHP;
        if (monsterHp.CurrentHP <= 0)
        {
            Hpbar.fillAmount = 0;
        }
    }
}
