using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LevelUp : MonoBehaviour
{
    // [ OtherSetting : Charactor �����Ұ� ]
    // 1) Model - Read/Write Check�ϱ�
    // 2) vfxgraph_LevelUp���� VFX Property(��ũ��Ʈ ������ �߰��ϱ�)����
    // Property Bindings���� Transform - Transform �߰��ϰ� ĳ���� �� �ֱ�

    public Animator anim; //ĳ���� animatior �ֱ�
    public VisualEffect levelUp; //vfxgraph_LevelUp �ֱ�

    [SerializeField]
    private bool levelingUp; //�������� �Ȱ��� Ȯ���� ����

    private void Update()
    {

        //LevelUp Test
        if (Input.GetButtonDown("Fire1") && !levelingUp)
        {
            levelUp.Play();
        }
            if (anim != null)
        {
            if (Input.GetButtonDown("Fire1") && !levelingUp) //Fire1 : left ctrl Ű
            {
                anim.SetTrigger("PowerUp"); //������ �ִϸ��̼�

                if(levelUp != null)
                {
                    levelUp.Play(); //������ ����Ʈ ���
                }

                levelingUp = true;
                StartCoroutine(ResetBool(levelingUp, 0.5f));
            }
        }
    }

    IEnumerator ResetBool(bool boolToReset, float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);

        levelingUp = !levelingUp; //������ �� �Ŀ� �ٽ� ������ Ȯ�� ���� �ʱ�ȭ
    }
}
