using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticFeedback : MonoBehaviour
{
    /*
스킬지속시간
    1.마법사 스킬
        [Staff_Fire]
            fireball : 터지는 이펙트 2f
            Meteor : 5f
            (메테오 떨어지는 시간 1.5f, 크레이터랑 연기 지속시간 맥시멈 2.5f
        [Staff_Ice]
            IceArrow : 터지는 이펙트 1f
            FrostColumns : 4.6f
            (3번에 걸쳐 얼음기둥(서릿발)이 1개, 2개, 3개 부채꼴 모양으로 순서대로 나감. 각각 딜레이 두번째 2개 .15f, 세번째 3개  .35f)
        [Staff_Elec]
            ChainLightning : 지속시간 3f, 터지는 이펙트 1f
            LightningStrike : 3.05f
            5번의 낙뢰가 떨어짐 지연시간은 1번째 .2f, 2번째 .4f, 3번째 .6f, 4번째 .8f, 5번째 1f
            (첫번째가 지연시간이 있는 이유는 위에 반짝하고 내려칠때 지연시간때문)
    2. 기사 스킬
        기본스킬 콤보 3번
        무기숙련스킬 콤보 5번
        [ArmingSword]
            Slash : .5f
            Triple Slash : .7f
            각각 0f, .15f, .3f 딜레이로 슬래쉬가 3번 나감
        [Katana]
            Blade : .7f
            각각 0f, .15f, .3f 딜레이로 스킬이 3번 나감
            StormBlade : .8f
            각각 0f, .15f, .3f 딜레이로 겹쳐진 Blades가 3번 나감
        [Rapier]
            Thrust : .6f
            RepeatingThrust : 1.6f
            각각 .18f, .33f, .53f, .63f, .78f로 부채꼴 모양으로 5번 나감. 순서는  4 2 1 3 5
    3.보스 몬스터 스킬
        Stomp : 크레이터 기준으로 맥시멈 4.75 
        도넛 모양은 1.9f동안 퍼져나감
        Rush : 맥시멈 2f
        돌진할때 나오는 이펙트
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