using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;

public class Monster_StateMachine : MonoBehaviour
{
    #region Declaration & Definition
    [SerializeField] public GameObject weapon;

    // Set Distance for Stop Chasing
    [SerializeField] public float lostDistance;
    [SerializeField] public float attackDistance;

    // Get Transform for Calculate Distance
    protected Transform playerTransform;
    protected Transform monsterTransform;

    protected float distance;

    // Get Position Values to return to Home
    protected Vector3 firstPosition;

    // Get Component
    protected NavMeshAgent nmAgent;
    protected Animator anim;
    protected Material monsterMaterial;
    new protected Rigidbody rigidbody;

    // Skeleton Information
    public enum MonsterType { Normal, Boss }
    public MonsterType monsterType;

    protected float thisHP = 1;

    private float maxHP;
    private float curHP = 1;
    Hp hp;

    protected bool isDead = false;

    // Set Monster State
    public enum State { IDLE, CHASE, ATTACK, PATTERN, DIE }
    public State state = State.IDLE;

    // Check Attack Animation
    private bool isAttackAnimationPlaying = false;
    #endregion

    #region Unity Default Method
    private void Awake()
    {
        if (!this.name.Contains("Boss"))
        {
            // Get Transform
            monsterTransform = this.gameObject.GetComponent<Transform>();
            playerTransform = FindObjectOfType<XROrigin>().gameObject.GetComponent<Transform>();

            // Get First Position(Respawn Point) / Normal Monster Only
            firstPosition = this.gameObject.GetComponent<Transform>().position;

            // Get Component
            nmAgent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            monsterMaterial = GetComponentInChildren<SkinnedMeshRenderer>().material;
            rigidbody = GetComponent<Rigidbody>();

            maxHP = GetComponent<Hp>().MaxHp;
            curHP = GetComponent<Hp>().CurrentHp;

            // Start Coroutine
            StartCoroutine(CheckState());
            StartCoroutine(MonsterAction());
        }
    }

    private void Update()
    {
        curHP = GetComponent<Hp>().CurrentHp;
    }
    #endregion

    public void SetMonsterState(int state)
    {
        this.state = (State)state;
    }

    #region Method
    protected IEnumerator CheckState()
    {
        // Condition Check (Check Monster's HP)
        while (!isDead)
        {
            // Wait before Check State
            yield return new WaitForSeconds(0.1f);

            // Condition Check 1 (Is The Moster Alive?)
            if (curHP == 0)
                state = State.DIE;

            // Condition Check 2 (Monster is Dead)
            if (state == State.DIE) yield break;

            // Calculate distance between Player and Monster
            distance = Vector3.Distance(playerTransform.position, monsterTransform.position);

            // Condition Check 3 (Set MonsterAction by Distance)
            if (distance <= attackDistance)
            {
                if (monsterType == MonsterType.Boss)
                    state = State.PATTERN;

                else
                    state = State.ATTACK;
            }

            else if (distance <= lostDistance && !isAttackAnimationPlaying)
                    state = State.CHASE;

            else
                state = State.IDLE;
        }
    }

    protected IEnumerator MonsterAction()
    {
        while (!isDead)
        {
            switch (state)
            {
                case State.IDLE:
                    // Animator Parameter
                    anim.SetBool("bIsChase", false);
                    anim.SetBool("bCanAttack", false);
                    maxHP = GetComponent<Hp>().MaxHp;
                    curHP = maxHP;
                    // Stop NavMeshAgent
                    this.nmAgent.ResetPath();
                    this.nmAgent.isStopped = true;
                    this.nmAgent.updatePosition = false;
                    this.nmAgent.updateRotation = false;
                    this.nmAgent.velocity = Vector3.zero;
                    break;
                case State.CHASE:
                    // Animator Parameter
                    anim.SetBool("bIsChase", true);
                    anim.SetBool("bCanAttack", false);
                    // Start NavMeshAgent (Monster -> Player)
                    this.nmAgent.SetDestination(playerTransform.position);
                    this.nmAgent.isStopped = false;
                    this.nmAgent.updatePosition = true;
                    this.nmAgent.updateRotation = true;
                    break;
                case State.ATTACK:
                    // Stop NavMeshAgent
                    this.nmAgent.isStopped = true;
                    this.nmAgent.updatePosition = false;
                    this.nmAgent.updateRotation = false;
                    // Animator Parameter
                    anim.SetBool("bCanAttack", true);
                    break;
                case State.DIE:
                    // Animator Parameter
                    anim.SetTrigger("tMonsterDie");
                    // Destory Monster
                    isDead = true;
                    nmAgent.isStopped = true;
                    GetComponent<CapsuleCollider>().enabled = false;
                    this.nmAgent.updatePosition = false;
                    this.nmAgent.updateRotation = false;
                    yield return new WaitForSeconds(3.0f);
                    // Turn Off Monster's Collider
                    Destroy(transform.parent.gameObject);
                    break;
                default:
                    break;
            }

            // Wait next State
            yield return new WaitForSeconds(0.1f);
        }
    }

    protected IEnumerator MonsterChangeColor()
    {
        monsterMaterial.color = Color.yellow;

        yield return new WaitForSeconds(0.3f);

        if (GetComponent<Hp>().CurrentHp == 0)
            monsterMaterial.color = Color.gray;

        else
            monsterMaterial.color = new Color(1, 1, 1);
    }

    public void HitState()
    {
        if (GetComponent<Hp>().CurrentHp == 0)
        {
            StartCoroutine("MonsterChangeColor");

            return;
        }

        StopAllCoroutines();
        anim.SetTrigger("tHit");
        StartCoroutine("MonsterChangeColor");
    }

    // temporary Set State.Die
    public void Adios()
    {
        thisHP = 0;
    }

    // Call When Player Is Dead
    private void OnPlayerDie()
    {
        // Stop All Moster's Coroutines
        StopAllCoroutines();

        //Stop Trace, Play Animation
        this.nmAgent.isStopped = true;
        anim.SetTrigger("tPlayerDie");
    }

    private void GoBackHome()
    {
        StopAllCoroutines();

        // Reset NavMeshAgent
        this.nmAgent.isStopped = true;
        this.nmAgent.ResetPath();
        this.nmAgent.stoppingDistance = 0;

        // Turn Off Collider
        GetComponent<CapsuleCollider>().enabled = false;

        // ReTarget NavMeshAgent (Monster -> Home)
        this.nmAgent.SetDestination(firstPosition);
        this.nmAgent.isStopped = false;
        this.nmAgent.updatePosition = true;
        this.nmAgent.updateRotation = true;
        this.nmAgent.stoppingDistance = 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GoBackHome();
        }

        else if (other.CompareTag("MonsterHome"))
        {
            StartCoroutine(CheckState());
            StartCoroutine(MonsterAction());

            GetComponent<CapsuleCollider>().enabled = true;
            state = State.IDLE;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    // Used for Animation Event
    public void SetMonsterHeadBeforeAttack()
    {
        Vector3 direction = (playerTransform.position - monsterTransform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    // Used for Animation Event
    // Check Attack Animation Start Point
    public void OnAttackAnimationStart()
    {
        isAttackAnimationPlaying = true;
    }

    // Used for Animation Event
    // Check Attack Animation End Point
    public void OnAttackAnimationEnd()
    {
        isAttackAnimationPlaying = false;

    }
    public void OnHittedAnimationEnd()
    {
        StartCoroutine(CheckState());
        StartCoroutine(MonsterAction());
    }

    // Set Gizmos for the Test
    protected void OnDrawGizmos()
    {
        if (state == State.CHASE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, lostDistance);
        }

        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }
    #endregion
}