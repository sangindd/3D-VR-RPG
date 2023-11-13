using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;

/*
 Note
 
 */

public class Mon_Skeleton_StateMachine : MonoBehaviour
{
    #region Declaration & Definition
    // Set Distance for Stop Chasing
    [SerializeField] public float lostDistance;
    [SerializeField] public float attackDistance;

    // Get Transform for Calculate Distance
    private Transform playerTransform;
    private Transform monsterTransform;

    private float distance;

    // Get Position Values to return to Home
    private Vector3 firstPosition;

    // Get NavMeshAgent & Animator
    private NavMeshAgent nmAgent;
    private Animator anim;

    // Skeleton Information
    private float maxHP = 100;
    private float curHP;

    private bool isDead = false;

    // Set Monster State
    public enum State { IDLE, CHASE, ATTACK, DIE }
    public State state = State.IDLE;

    // Check Attack Animation
    private bool isAttackAnimationPlaying = false;
    #endregion

    #region Unity Default Method
    private void Start()
    {
        // Get Transform
        monsterTransform = this.gameObject.GetComponent<Transform>();
        playerTransform = FindObjectOfType<XROrigin>().gameObject.GetComponent<Transform>();
        // Get First Position(Respawn Point)
        firstPosition = this.gameObject.GetComponent<Transform>().position;

        // Get Component
        nmAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        // Set Skeleton's Status
        curHP = maxHP;

        // Start Coroutine
        StartCoroutine(CheckState());
        StartCoroutine(MonsterAction());
    }
    #endregion

    public void SetMonsterState(int state)
    {
        this.state = (State)state;
    }

    #region Method
    IEnumerator CheckState()
    {
        // Condition Check (Check Monster's HP)
        while (!isDead)
        {
            // Wait before Check State
            yield return new WaitForSeconds(0.1f);

            // Condition Check 1 (Monster is Dead)
            if (state == State.DIE) yield break;

            // Calculate distance between Player and Monster
            distance = Vector3.Distance(playerTransform.position, monsterTransform.position);

            // Condition Check 2 (Set MonsterAction by Distance)
            if (distance <= attackDistance)
            {
                state = State.ATTACK;
            }

            else if (distance <= lostDistance && !isAttackAnimationPlaying)
                state = State.CHASE;

            else
                state = State.IDLE;
        }
    }

    IEnumerator MonsterAction()
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
                    // Turn Off Monster's Collider
                    GetComponent<CapsuleCollider>().enabled = false;
                    // Destory Monster
                    isDead = true;
                    nmAgent.isStopped = true;
                    yield return new WaitForSeconds(3.0f);
                    Destroy(this.gameObject);
                    break;
                default:
                    break;
            }

            // Wait next State
            yield return new WaitForSeconds(0.3f);
        }
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
    private void OnDrawGizmos()
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