using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hp : MonoBehaviour
{

    float Init_Hp;
    [SerializeField]
    private float maxHp = 100f;
    public float MaxHp { get { return maxHp; } }
    [SerializeField]
    private float hp;
    public float CurrentHp { get { return hp; } }
    int str;
    float level;

    bool isHit = false;
    public UnityEvent OnHit;
    public UnityEvent OnHeal;
    public UnityEvent OnDestroy;
    public UnityEvent OnMonsterDestroy;
    public UnityEvent OnPlayerDie;

    //데미지 표현
    [SerializeField]
    Transform dmgPos;
    [SerializeField]
    GameObject dmg;

    private void Awake()
    {
        //hp 계산
        Init();
        try
        {
            str = GetComponent<Stat>().Strength;
        }
        catch
        {

        }

        level = GameManager.Instance.Level;
        Init_Hp = maxHp;
    }

    private void Update()
    {
        if (hp > maxHp)
            hp = maxHp;
        if (GetComponent<Stat>() != null)
        {
            maxHp = 100f + ((GetComponent<Stat>().Strength - 1) * 4f);
            if (GetComponent<Stat>().Strength > str)
            {
                hp += (GetComponent<Stat>().Strength - str) * 4f;
                str = GetComponent<Stat>().Strength;
            }
        }
        if (CompareTag("Monster") && !name.Contains("Boss"))
        {
            if (level != GameManager.Instance.Level)
            {
                level = GameManager.Instance.Level;
                if (hp == maxHp)
                {
                    maxHp = Init_Hp + Init_Hp * (GameManager.Instance.Level * 0.3f);
                    hp = maxHp;
                }
            }
        }
    }

    public void Init()
    {
        maxHp = maxHp + maxHp * (GameManager.Instance.Level * 0.3f);
        hp = maxHp;
    }

    public void Hit(float value)
    {
        if (!isHit)
        {
            //StartCoroutine(HitActive(value));
            if (GetComponent<Stat>() != null)
            {
                value -= GetComponent<Stat>().Defence;
                if (value <= 0)
                {
                    value = 1f;
                }
            }
            hp -= value;
            try
            {
                var text = Instantiate(dmg, dmgPos);
                if (GetComponent<Player>() != null)
                    text.GetComponent<TextMesh>().text = "-" + value.ToString();
                else
                    text.GetComponent<TextMesh>().text = value.ToString();
            }
            catch { }
            if (hp < 0)
                hp = 0;

            OnHit?.Invoke();
            if (hp == 0)
            {
                OnDestroy?.Invoke();

                //target.Destroy();
                if (GetComponent<Player>() == null)
                    OnMonsterDestroy?.Invoke();
                //Destroy(gameObject);
                else
                {
                    OnPlayerDie?.Invoke();
                    print("Player Die");
                }
                return;
            }
        }
    }

    public float HpRatio()
    {
        return hp / maxHp;
    }

    public void Heal()
    {
        hp += maxHp * 0.5f;
        OnHeal?.Invoke();
    }

    IEnumerator HitActive(float value)
    {
        hp -= value;
        isHit = true;
        yield return new WaitForSeconds(0.1f);
        isHit = false;
    }
}
