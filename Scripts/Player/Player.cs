using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
public class Player : MonoBehaviour
{
    public enum Type
    {
        Knight,
        Sorcerer
    }

    public Type jobType;

    //돈 관련
    [SerializeField]
    int coin = 0;
    public int Coin
    {
        get
        {
            return coin;
        }
        set
        {
            coin = value;
        }
    }

    //경험치관련
    public UnityEvent OnLevelUp;
    public UnityEvent OnGetEXP;
    [SerializeField]
    double maxExp = 6;
    public double MaxExp { get { return maxExp; } }  //외부참조용
    [SerializeField]
    double curExp = 0;
    public double CurExp { get { return curExp; } }

    //스탯관련
    [SerializeField]
    Stat stat;
    public float dmg = 1f;
    public float cri = 0f;
    public float cdr = 0f;

    int intelligence;

    [SerializeField]
    GameObject blood;
    [SerializeField]
    public Transform coolPos;

    IEnumerator enumerator;

    private void Start()
    {
        blood.SetActive(false);
        StartCoroutine(OnBounce());
        intelligence = stat.Intelligence;
    }

    void Update()
    {
        //평타
        if (jobType == Type.Knight)
            dmg = 1 + ((stat.Strength - 1) * .5f);
        cri = 0 + ((stat.Dexterity - 1) * 1.5f);
        if (jobType == Type.Sorcerer)
            cdr = 0 + ((stat.Intelligence - 1) * 1.5f);
        else
        {
            if (intelligence < stat.Intelligence)
            {
                stat.Defence += (stat.Intelligence - 1) * .5f;
                intelligence = stat.Intelligence;
            }
            else if (intelligence > stat.Intelligence)
            {
                stat.Defence -= (intelligence - stat.Intelligence) * .5f;
                intelligence = stat.Intelligence;
            }
        }
    }

    //직업 설정
    public void SetType(float t)
    {
        jobType = (Type)t;
    }

    //레벨업
    public void GetEXP(float exp)
    {
        curExp += exp;
        if (curExp >= maxExp)
        {
            LevelUp();
        }
        OnGetEXP?.Invoke();
    }

    public void LevelUp()
    {
        curExp -= maxExp;
        maxExp = Math.Round(maxExp * 1.5f);
        GameManager.Instance.Level++;
        OnLevelUp?.Invoke();
        stat.LevelUp();
    }

    public void GetCoin(int coin)
    {
        Coin += coin;
    }

    public void Blood()
    {
        enumerator = OnBlood();
        if (GetComponent<Hp>().HpRatio() > 0.5)
        {
            StopCoroutine(enumerator);
            StartCoroutine(enumerator);
        }
    }

    IEnumerator OnBlood()
    {
        blood.SetActive(true);
        yield return new WaitForSeconds(.2f);
        blood.SetActive(false);
    }

    IEnumerator OnBounce()
    {
        while (true)
        {
            if (GetComponent<Hp>().HpRatio() <= 0.5)
            {
                if (!blood.GetComponent<AudioSource>().isPlaying)
                    blood.GetComponent<AudioSource>().Play();
                blood.SetActive(true);
                //blood.SetActive(false);
                var material = blood.transform.GetChild(0).GetComponent<MeshRenderer>().material;

                var value = 2f - (0.16f * (1f - GetComponent<Hp>().HpRatio()));
                material.SetFloat("_BloodAlpha", value);
                if (GetComponent<Hp>().HpRatio() <= 0.3)
                {
                    material.SetFloat("_CenterRadius", 3f);
                    yield return new WaitForSeconds(.2f);
                    material.SetFloat("_CenterRadius", 3.5f);
                }
            }
            else
            {
                blood.SetActive(false);
            }
            yield return new WaitForSeconds(.2f);
        }
    }
}
