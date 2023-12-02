using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Boss : MonoBehaviour
{
    #region Declaration & Definition
    // Set Distance for Stop Chasing
    [SerializeField] public float lostDistance;
    [SerializeField] public float attackDistance;
    [SerializeField] GameObject stomp;
    [SerializeField] GameObject weapon;
    [SerializeField] Transform weaponPos;
    [SerializeField] Transform drawPos;

    // Get Transform for Calculate Distance
    protected Transform playerTransform;
    protected Vector3 InitPosition;
    protected Quaternion InitRotation;

    protected float distance;

    // Get NavMeshAgent & Animator
    [SerializeField]
    protected NavMeshAgent nmAgent;
    protected Animator anim;

    protected float thisHP = 1;

    protected bool isDead = false;

    [SerializeField]
    float Danger;
    public bool isGoHome = false;
    public bool isPattern = false;
    bool isAttack = false;
    float initSpeed;

    [SerializeField]
    GameObject Skill;
    [SerializeField]
    GameObject Door;
    [SerializeField]
    Rigidbody body;

    #endregion

    //#region Unity Default Method
    private void Awake()
    {
        // Get First Position(Respawn Point) / Normal Monster Only
        InitPosition = transform.position;
        InitRotation = transform.rotation;

        // Get Transform
        playerTransform = FindObjectOfType<XROrigin>().gameObject.GetComponent<Transform>();

        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        initSpeed = nmAgent.speed;
        nmAgent.isStopped = true;
    }

    private void Update()
    {
        distance = Vector3.Distance(playerTransform.position, transform.position);

        if (!isPattern && nmAgent.enabled)
        {
            if (isGoHome)
            {
                isGoHome = true;
                nmAgent.SetDestination(InitPosition);
                nmAgent.isStopped = false;
                if (nmAgent.remainingDistance <= 1f)
                {
                    anim.SetTrigger("IsIdle");
                    transform.rotation = InitRotation;
                    StopAllCoroutines();
                    isGoHome = false;
                    anim.ResetTrigger("IsWalk");
                    nmAgent.isStopped = true;
                    lostDistance = 15f;
                }
                else
                    anim.SetTrigger("IsWalk");
            }
            else if (!isGoHome)
            {
                SetTarget();

                if (drawPos.childCount > 0)
                    if (GetComponent<Hp>().CurrentHp / GetComponent<Hp>().MaxHp <= 0.5f)
                    {
                        isPattern = true;
                        nmAgent.enabled = false;
                        anim.SetTrigger("IsDraw");
                    }
                if (Danger > 700)
                {
                    nmAgent.enabled = false;
                    anim.SetTrigger("IsActivate");
                    //nmAgent.enabled = false;
                    Danger = 0;

                    //SetTarget();
                }

                else if (distance < attackDistance)
                {
                    if (!isAttack)
                    {
                        SetMonsterHeadBeforeAttack();
                        isAttack = true;
                    }
                    nmAgent.isStopped = true;
                    anim.SetTrigger("IsAttackMelee");
                    anim.ResetTrigger("IsRun");
                    Danger -= 1;
                }
                else if (distance <= lostDistance)
                {
                    lostDistance = 60;
                    nmAgent.isStopped = false;
                    anim.SetTrigger("IsRun");
                    anim.ResetTrigger("IsAttackMelee");
                    Danger++;
                }
                else
                {

                }
            }
        }
        else
        {
            Danger = 0;
        }
    }
    void OnPattern()
    {
        //anim.SetTrigger("IsStomp");
        //anim.SetTrigger("IsRush");
        //OnRush();
        if (Random.Range(0, 3) == 0)
        {
            anim.SetTrigger("IsRush");
            OnRush();
        }
        else if (Random.Range(0, 3) == 1)
            anim.SetTrigger("IsStomp");
        else
            anim.SetTrigger("IsJump");
    }

    void OnRush()
    {
        print("OnRush");
        nmAgent.enabled = false;
        //FindAnyObjectByType<Boss>().gameObject.GetComponent<Rigidbody>().AddRelativeForce((transform.forward) * 500f);
        body.AddForce((transform.forward) * 700f);
        Skill.SetActive(true);
        Invoke("OnEndPattern", 2f);
        //var destination = transform.position + (transform.forward * 20f);
        //StartCoroutine(CheckPos(destination));
    }
    void OnStomp()
    {
        print("OnStomp");
        var skill = Instantiate(stomp, transform);
        Destroy(skill, 3f);
        OnEndPattern();
    }
    void OnLanding()
    {
        print("OnLanding");
        nmAgent.enabled = false;
        body.AddForce((transform.forward + transform.up) * 500f);
    }

    IEnumerator CheckPos(Vector3 destination)
    {
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            if (Vector3.Distance(destination, transform.position) <= 1f)
            {
                break;
            }
            else
            {
                transform.position += transform.forward * 0.5f;
            }
        }
        OnEndPattern();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Pillar") && isPattern)
        {
            anim.SetTrigger("IsStunned");
        }
    }

    protected void OnDrawGizmos()
    {
        if (distance <= lostDistance)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, lostDistance);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Pillar") && isPattern)
        {
            anim.SetTrigger("IsStunned");
            SetTarget();
            isPattern = false;
        }
    }

    void SetMonsterHeadBeforeAttack()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void SetGravity()
    {
        body.useGravity = !body.useGravity;
    }

    public void SetTarget()
    {
        if (nmAgent.enabled == true)
            nmAgent.SetDestination(playerTransform.position);
        if(Danger<700)
            Danger += .5f;
    }

    void OnEndAttack()
    {
        isAttack = false;
    }

    void OnDead()
    {
        nmAgent.isStopped = true;
    }

    void OnStartPattern()
    {
        isPattern = true;
        nmAgent.enabled = false;
        nmAgent.updatePosition = false;
        nmAgent.updateRotation = false;
        anim.ResetTrigger("IsRun");
    }

    void OnEndPattern()
    {
        StopAllCoroutines();
        OffSkill();
        isPattern = false;
        nmAgent.enabled = true;
        nmAgent.updatePosition = true;
        nmAgent.updateRotation = true;
    }

    void OffSkill()
    {
        Skill.SetActive(false);
    }

    void DrawWeapon()
    {
        weapon.transform.SetParent(weaponPos);
        weapon.transform.localPosition = Vector3.zero;
        //weapon.transform.Rotate(Vector3.zero);
        attackDistance = 7f;
        Danger = 700;
        OnEndPattern();
    }
}