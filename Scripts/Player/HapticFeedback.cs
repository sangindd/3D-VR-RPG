using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticFeedback : MonoBehaviour
{
    /*
��ų���ӽð�
    1.������ ��ų
        [Staff_Fire]
            fireball : ������ ����Ʈ 2f
            Meteor : 5f
            (���׿� �������� �ð� 1.5f, ũ�����Ͷ� ���� ���ӽð� �ƽø� 2.5f
        [Staff_Ice]
            IceArrow : ������ ����Ʈ 1f
            FrostColumns : 4.6f
            (3���� ���� �������(������)�� 1��, 2��, 3�� ��ä�� ������� ������� ����. ���� ������ �ι�° 2�� .15f, ����° 3��  .35f)
        [Staff_Elec]
            ChainLightning : ���ӽð� 3f, ������ ����Ʈ 1f
            LightningStrike : 3.05f
            5���� ���ڰ� ������ �����ð��� 1��° .2f, 2��° .4f, 3��° .6f, 4��° .8f, 5��° 1f
            (ù��°�� �����ð��� �ִ� ������ ���� ��¦�ϰ� ����ĥ�� �����ð�����)
    2. ��� ��ų
        �⺻��ų �޺� 3��
        ������ý�ų �޺� 5��
        [ArmingSword]
            Slash : .5f
            Triple Slash : .7f
            ���� 0f, .15f, .3f �����̷� �������� 3�� ����
        [Katana]
            Blade : .7f
            ���� 0f, .15f, .3f �����̷� ��ų�� 3�� ����
            StormBlade : .8f
            ���� 0f, .15f, .3f �����̷� ������ Blades�� 3�� ����
        [Rapier]
            Thrust : .6f
            RepeatingThrust : 1.6f
            ���� .18f, .33f, .53f, .63f, .78f�� ��ä�� ������� 5�� ����. ������  4 2 1 3 5
    3.���� ���� ��ų
        Stomp : ũ������ �������� �ƽø� 4.75 
        ���� ����� 1.9f���� ��������
        Rush : �ƽø� 2f
        �����Ҷ� ������ ����Ʈ
     */

    #region Declaration & Definition
    [Header("Controller")]
    [SerializeField] public ActionBasedController rightHand;
    [SerializeField] public ActionBasedController leftHand;
    #endregion

    #region Method
    // Common
    public void Attack(ActionBasedController controller)
    {
        controller.SendHapticImpulse(0.5f, 0.1f);
    }

    public void CallSkillHaptic(string Skill)
    {
        StartCoroutine(Skill);
    }

    private void UltimateMeleeAttack(float amplitude, float duration)
    {
        rightHand.SendHapticImpulse(amplitude, duration);
        leftHand.SendHapticImpulse(amplitude, duration);
    }

    // Monster
    IEnumerator MonsterIsHit()
    {
        rightHand.SendHapticImpulse(0.6f, 0.2f);
        yield return new WaitForSeconds(0.2f);
    }

    // Character
    public void CharacterIsHit()
    {
        rightHand.SendHapticImpulse(0.3f, 0.3f);
        leftHand.SendHapticImpulse(0.3f, 0.3f);
    }

    // Knight
    IEnumerator ArmingSword_Slash()
    {
        UltimateMeleeAttack(0.5f, 0.3f);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator ArmingSword_TripleSlash()
    {
        for (int i = 0; i < 3; i++)
        {
            UltimateMeleeAttack(0.6f, 0.15f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Katana_Blade()
    {
        UltimateMeleeAttack(0.5f, 0.5f);
        yield return new WaitForSeconds(0.7f);
    }

    IEnumerator Katana_StormBlade()
    {
        for (int i = 0; i < 3; i++)
        {
            UltimateMeleeAttack(0.6f, 0.15f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Thrust()
    {
        UltimateMeleeAttack(0.5f, 0.4f);
        yield return new WaitForSeconds(0.6f);
    }

    IEnumerator RepeatingThrust()
    {
        yield return new WaitForSeconds(0.18f);

        for (int i = 0; i < 5; i++)
        {
            UltimateMeleeAttack(0.6f, 0.15f);
            yield return new WaitForSeconds(0.16f);
        }
    }

    // Magician
    IEnumerator Meteor()
    {
        UltimateMeleeAttack(0.9f, 0.7f);

        yield return new WaitForSeconds(0.7f);
    }

    IEnumerator FrostColumns()
    {
        for (int i = 0; i < 3; i++)
        {
            UltimateMeleeAttack(0.6f, 0.2f);
            yield return new WaitForSeconds(0.22f);
        }
    }

    IEnumerator LightningStrike()
    {
        for (int i = 0; i < 3; i++)
        {
            UltimateMeleeAttack(0.6f, 0.15f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Casting()
    {
        rightHand.SendHapticImpulse(0.15f, 0.11f);
        yield return new WaitForSeconds(0.11f);

        rightHand.SendHapticImpulse(0.30f, 0.11f);
        yield return new WaitForSeconds(0.11f);

        rightHand.SendHapticImpulse(0.45f, 0.11f);
        yield return new WaitForSeconds(0.11f);

        rightHand.SendHapticImpulse(0.60f, 0.11f);
        yield return new WaitForSeconds(0.11f);

        rightHand.SendHapticImpulse(0.75f, 0.11f);
        yield return new WaitForSeconds(0.11f);

        rightHand.SendHapticImpulse(0.75f, 0.11f);
        yield return new WaitForSeconds(0.11f);

        rightHand.SendHapticImpulse(0.60f, 0.11f);
        yield return new WaitForSeconds(0.11f);

        rightHand.SendHapticImpulse(0.45f, 0.11f);
        yield return new WaitForSeconds(0.11f);

        rightHand.SendHapticImpulse(0.30f, 0.11f);
        yield return new WaitForSeconds(0.11f);

        rightHand.SendHapticImpulse(0.15f, 0.11f);
        yield return new WaitForSeconds(0.11f);
    }
    #endregion
}