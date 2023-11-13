using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LevelUp : MonoBehaviour
{
    // [ OtherSetting : Charactor 설정할것 ]
    // 1) Model - Read/Write Check하기
    // 2) vfxgraph_LevelUp에서 VFX Property(스크립트 없으면 추가하기)에서
    // Property Bindings에서 Transform - Transform 추가하고 캐릭터 모델 넣기

    public Animator anim; //캐릭터 animatior 넣기
    public VisualEffect levelUp; //vfxgraph_LevelUp 넣기

    [SerializeField]
    private bool levelingUp; //레벨업이 된건지 확인할 변수

    private void Update()
    {

        //LevelUp Test
        if (Input.GetButtonDown("Fire1") && !levelingUp)
        {
            levelUp.Play();
        }
            if (anim != null)
        {
            if (Input.GetButtonDown("Fire1") && !levelingUp) //Fire1 : left ctrl 키
            {
                anim.SetTrigger("PowerUp"); //레벨업 애니메이션

                if(levelUp != null)
                {
                    levelUp.Play(); //레벨업 이펙트 재생
                }

                levelingUp = true;
                StartCoroutine(ResetBool(levelingUp, 0.5f));
            }
        }
    }

    IEnumerator ResetBool(bool boolToReset, float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);

        levelingUp = !levelingUp; //레벨업 한 후에 다시 레벨업 확인 변수 초기화
    }
}
