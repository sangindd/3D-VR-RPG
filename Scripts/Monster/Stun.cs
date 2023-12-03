using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stun : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Pillar") && transform.GetChild(0).GetComponent<Boss>().isPattern)
        {
            anim.SetTrigger("IsStunned");
            transform.GetChild(0).GetComponent<Boss>().SetTarget();
            transform.GetChild(0).GetComponent<Boss>().isPattern = false;
        }
    }
}
