using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    public UnityEvent OnEndDelay;
    public UnityEvent OnSkillAvailable;
    public UnityEvent OnSkillUnavailable;

    [SerializeField]
    InputActionAsset action;
    [SerializeField]
    public Slash Skill;
    [SerializeField]
    Damage Dmg;
    [SerializeField]
    float delay = 1f;
    [SerializeField]
    int combo = 0;

    [SerializeField]
    Animator anim;

    public Weapon weapon;
    bool isActivate;
    public bool isDelay = false;

    public UnityEvent OnHitHaptic;
    public UnityEvent OnSkillHaptic;
    public UnityEvent OnSkillHaptic2;


    void Start()
    {
        weapon = GetComponent<Weapon>();
        //DB.ReadWeaponData(gameObject.name, weapon);

        action.FindActionMap("XRI RightHand Interaction").FindAction("Activate").performed += Activate;
        action.FindActionMap("XRI RightHand Interaction").FindAction("Activate").canceled += CancleActivate;
    }

    private void Activate(InputAction.CallbackContext obj)
    {
        isActivate = true;
    }

    private void CancleActivate(InputAction.CallbackContext obj)
    {
        isActivate = false;
    }

    void Update()
    {
        if (combo >= 3)
        {
            OnSkillAvailable?.Invoke();
            if (combo >= 6)
            {
                transform.GetChild(0).transform.GetChild(2).transform.DOScaleY(1.5f, 0.1f);
            }
            else
                transform.GetChild(0).transform.GetChild(2).transform.DOScaleY(0.5f, 0.1f);
        }
        else
        {
            OnSkillUnavailable?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<Staff>() == null)
        {
            if (!isDelay && collision.gameObject.GetComponent<Hittable>() != null)
            {
                combo++;
                OnHitHaptic?.Invoke();
            }
            if (combo >= 6 && isActivate)
            {
                var r = transform.rotation;
                r.x = 0;
                r.z = 0;
                var skill = Instantiate(Skill.Slash2, transform.position, r);

                skill.GetComponent<ParticleSystem>().Play();
                combo -= 6;
                OnSkillHaptic2?.Invoke();
                Destroy(skill, 1f);
            }
            else if (combo >= 3 && isActivate)
            {
                var skill = Instantiate(Skill.Slash1, transform);
                skill.transform.SetParent(null);
                var r = skill.transform.rotation;
                r.x = 0;
                r.z = 0;
                skill.GetComponent<ParticleSystem>().Play();
                combo -= 3;
                OnSkillHaptic?.Invoke();
                Destroy(skill, 1.5f);
            }
        }
    }

    public void OnDelay()
    {
        isDelay = true;
        StopAllCoroutines();
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        OnEndDelay?.Invoke();
        isDelay = false;
    }

    public void PlayAnim()
    {
        anim.SetFloat("Speed", UnityEngine.Random.Range(0f,0.3f));
    }
}
