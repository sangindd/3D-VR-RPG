using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Monster_StateMachine2 : MonoBehaviour
{
    #region Declaration & Definition
    // Set Distance for Stop Chasing
    [SerializeField] public float lostDistance;
    [SerializeField] public float attackDistance;

    // Get Transform for Calculate Distance
    protected Transform playerTransform;
    protected Transform monsterTransform;

    protected float distance;

    // Get Position Values to return to Home
    protected Vector3 firstPosition;

    // Get NavMeshAgent & Animator
    protected NavMeshAgent nmAgent;
    protected Animator anim;

    // Skeleton Information
    public enum MonsterType { Normal, Boss }
    public MonsterType monsterType;

    [SerializeField] private float maxHP = 100.0f;
    public float MaxHP { get { return maxHP; } }
    [SerializeField] private float curHP;
    public float CurrentHP { get { return curHP; } }

    protected bool isHit = false;
    protected bool isDead = false;

    protected float thisHP = 1;

    public UnityEvent OnHit;
    public UnityEvent OnMonsterDestroy;

    // Set Monster State
    public enum State { IDLE, CHASE, ATTACK, PATTERN, DIE }
    public State state = State.IDLE;

    // Check Attack Animation
    private bool isAttackAnimationPlaying = false;
    #endregion

    #region Unity Default Method
    private void Awake()
    {
        // Set HP
        curHP = MaxHP;

        // Get Transform
        monsterTransform = this.gameObject.GetComponent<Transform>();
        playerTransform = FindObjectOfType<XROrigin>().gameObject.GetComponent<Transform>();

        // Get First Position(Respawn Point) / Normal Monster Only
        firstPosition = this.gameObject.GetComponent<Transform>().position;

        // Get Component
        nmAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Start Coroutine
        StartCoroutine(CheckState());
        StartCoroutine(MonsterAction());
    }

    private void Update()
    {
        if (curHP > maxHP)
            curHP = maxHP;
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
            if (curHP <= 0)
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
                    // Turn off Monster
                    isDead = true;
                    nmAgent.isStopped = true;
                    this.nmAgent.updatePosition = false;
                    this.nmAgent.updateRotation = false;
                    yield return new WaitForSeconds(1.5f);
                    GetComponent<CapsuleCollider>().enabled = false;
                    OnMonsterDestroy?.Invoke();
                    // Turn On Monster
                    yield return new WaitForSeconds(0.5f);
                    isDead = false;
                    GetComponent<CapsuleCollider>().enabled = true;
                    curHP = maxHP;
                    state = State.IDLE;
                    transform.root.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }

            // Wait next State
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Hit(float value)
    {
        curHP -= value;

        OnHit?.Invoke();
    }

    private void ResetHP()
    {
        curHP = maxHP;
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