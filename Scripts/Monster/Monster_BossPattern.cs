using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;

/* Note
 asdweqssad
 
 */


public class Monster_BossPattern : Monster_StateMachine
{
    #region Declaration & Definition
    // Get GameObject
    //[SerializeField] public GameObject projectile;
    [SerializeField] public GameObject doughnut;
    [SerializeField] public GameObject Shield;

    // Set Skill Effect Point
    //public Transform projectilePos;
    public Transform doughnutPos;
    public Transform ShieldPos;

    // Where The Boss Jumps
    Vector3 estimatedVector;

    // Set Boss Pattern
    private enum Pattern { A, B, C, D, E }
    private Pattern pattern;


    #endregion


    #region Unity Default Method
    private void Awake()
    {
        // Get Transform to Calculate Distance
        monsterTransform = this.gameObject.GetComponent<Transform>();
        playerTransform = FindObjectOfType<XROrigin>().gameObject.GetComponent<Transform>();

        // Get Component
        nmAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Start Coroutine
        StartCoroutine(CheckState());
        StartCoroutine(MonsterAction());
    }

    void Update()
    {

    }
    #endregion

    #region Method

    // Pattern 1
    //private IEnumerator Throw()
    //{
    //    yield return null;
    //}

    // Pattern 2
    //private IEnumerator Stamp()
    //{
    //    yield return null;
    //}

    // Pattern 3
    //private IEnumerator Dash()
    //{
    //    yield return null;
    //}

    ////////////////////////////////////////////////////////////////////////////////////////////
    // Used for Animation Event


    #endregion
}